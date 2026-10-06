using System.Text.Json;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using static ProjectEnergy.Web.Content.ContentAliases;

namespace ProjectEnergy.Web.Content;

public class DemoContentComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
        => builder.AddNotificationAsyncHandler<UmbracoApplicationStartedNotification, DemoContentSeeder>();
}

/// <summary>
/// Cria, numa base de dados vazia, o modelo de conteúdo (idiomas, dicionário, tipos de documento, templates)
/// e o conteúdo de demonstração. Não faz nada se o tipo "homePage" já existir — a partir daí o conteúdo
/// é gerido no backoffice e nunca é sobrescrito por este código.
/// </summary>
public class DemoContentSeeder(
    IRuntimeState runtimeState,
    ILanguageService languageService,
    IDictionaryItemService dictionaryItemService,
    IDataTypeService dataTypeService,
    IContentTypeService contentTypeService,
    ITemplateService templateService,
    IContentService contentService,
    IDomainService domainService,
    IMetricsConsentService metricsConsentService,
    IShortStringHelper shortStringHelper,
    IWebHostEnvironment environment,
    ILogger<DemoContentSeeder> logger)
    : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private const string Pt = "pt";
    private const string En = "en";
    private static readonly Guid User = Constants.Security.SuperUserKey;

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {
        if (runtimeState.Level != RuntimeLevel.Run || contentTypeService.Get(HomePage) is not null)
        {
            return;
        }

        logger.LogInformation("Base de dados sem modelo Project Energy: a criar modelo e conteúdo de demonstração.");

        // Telemetria Umbraco no nível mínimo (sem dados de utilização) até haver decisão sobre analytics.
        await metricsConsentService.SetConsentLevelAsync(TelemetryLevel.Minimal);

        var (pt, en) = await EnsureLanguagesAsync();
        await CreateDictionaryAsync(pt, en);
        var types = await CreateDocumentTypesAsync();
        await CreateContentAsync(types);

        logger.LogInformation("Modelo e conteúdo de demonstração criados.");
    }

    private async Task<(ILanguage Pt, ILanguage En)> EnsureLanguagesAsync()
    {
        var pt = await languageService.GetAsync(Pt);
        if (pt is null)
        {
            pt = new Language(Pt, "Português") { IsDefault = true, IsMandatory = true };
            await Expect(languageService.CreateAsync(pt, User), "idioma pt");
        }

        var en = await languageService.GetAsync(En);
        if (en is null)
        {
            en = new Language(En, "English") { IsMandatory = true };
            await Expect(languageService.CreateAsync(en, User), "idioma en");
        }

        // O instalador cria en-US por omissão; o site usa apenas pt e en.
        if (await languageService.GetAsync("en-US") is not null)
        {
            await Expect(languageService.DeleteAsync("en-US", User), "remover en-US");
        }

        return ((await languageService.GetAsync(Pt))!, (await languageService.GetAsync(En))!);
    }

    private async Task CreateDictionaryAsync(ILanguage pt, ILanguage en)
    {
        foreach (var (key, text) in DemoContent.Dictionary)
        {
            if (await dictionaryItemService.GetAsync(key) is not null)
            {
                continue;
            }

            var item = new DictionaryItem(key)
            {
                Translations = [new DictionaryTranslation(pt, text.Pt), new DictionaryTranslation(en, text.En)],
            };
            await Expect(dictionaryItemService.CreateAsync(item, User), $"dicionário {key}");
        }
    }

    private sealed record DocTypes(IContentType Home, IContentType Content, IContentType Services, IContentType Service, IContentType Careers, IContentType Contact);

    private async Task<DocTypes> CreateDocumentTypesAsync()
    {
        var textstring = (await dataTypeService.GetAsync(Constants.DataTypes.Guids.TextstringGuid))!;
        var textarea = (await dataTypeService.GetAsync(Constants.DataTypes.Guids.TextareaGuid))!;
        var richText = (await dataTypeService.GetAsync(Constants.DataTypes.Guids.RichtextEditorGuid))!;
        var checkbox = (await dataTypeService.GetAsync(Constants.DataTypes.Guids.CheckboxGuid))!;

        void AddPageProperties(ContentType type)
        {
            type.AddPropertyGroup("content", "Conteúdo");
            AddProperty(type, textarea, Props.Intro, "Introdução", "content");
            AddProperty(type, richText, Props.Body, "Corpo", "content");
            AddSeoAndDemo(type);
        }

        void AddSeoAndDemo(ContentType type)
        {
            type.AddPropertyGroup("seo", "SEO");
            AddProperty(type, textarea, Props.MetaDescription, "Meta description", "seo",
                "Resumo para motores de busca (cerca de 150 caracteres).");
            type.AddPropertyGroup("status", "Estado");
            AddProperty(type, checkbox, Props.IsDemoContent, "Conteúdo de demonstração", "status",
                "Desmarcar quando o conteúdo desta página estiver aprovado.", variant: false);
        }

        var service = NewType(ServiceItem, "Serviço", "icon-settings");
        service.AddPropertyGroup("content", "Conteúdo");
        AddProperty(service, textarea, Props.Summary, "Resumo", "content");
        AddProperty(service, checkbox, Props.IsPlanned, "Capacidade planeada", "content",
            "Marcar se o serviço ainda não é oferecido (será apresentado como capacidade futura).", variant: false);
        AddProperty(service, textstring, Props.Icon, "Ícone", "content",
            "workers, maintenance, welding, fabrication, composites, coating, safety, training", variant: false);
        await Expect(contentTypeService.CreateAsync(service, User), ServiceItem);

        var content = await CreatePageTypeAsync(ContentPage, "Página de conteúdo", "icon-document", AddPageProperties);
        var careers = await CreatePageTypeAsync(CareersPage, "Página de carreiras", "icon-users", AddPageProperties);
        var contact = await CreatePageTypeAsync(ContactPage, "Página de contacto", "icon-message", AddPageProperties);
        var services = await CreatePageTypeAsync(ServicesPage, "Página de serviços", "icon-box", type =>
        {
            AddPageProperties(type);
            type.AllowedContentTypes = [new ContentTypeSort(service.Key, 0, service.Alias)];
        });

        var home = await CreatePageTypeAsync(HomePage, "Página inicial", "icon-home", type =>
        {
            type.AllowedAsRoot = true;
            type.AllowedContentTypes =
            [
                new ContentTypeSort(content.Key, 0, content.Alias),
                new ContentTypeSort(services.Key, 1, services.Alias),
                new ContentTypeSort(careers.Key, 2, careers.Alias),
                new ContentTypeSort(contact.Key, 3, contact.Alias),
            ];

            type.AddPropertyGroup("hero", "Destaque");
            AddProperty(type, textstring, Props.HeroTitle, "Título principal", "hero");
            AddProperty(type, textarea, Props.HeroText, "Mensagem de apoio", "hero");

            type.AddPropertyGroup("sections", "Secções");
            AddProperty(type, textstring, Props.IntroTitle, "Apresentação — título", "sections");
            AddProperty(type, textarea, Props.IntroText, "Apresentação — texto", "sections");
            AddProperty(type, textstring, Props.HseqTitle, "HSEQ — título", "sections");
            AddProperty(type, textarea, Props.HseqText, "HSEQ — texto", "sections");
            AddProperty(type, textstring, Props.LocalContentTitle, "Conteúdo local — título", "sections");
            AddProperty(type, textarea, Props.LocalContentText, "Conteúdo local — texto", "sections");
            AddProperty(type, textstring, Props.CareersTitle, "Carreiras — título", "sections");
            AddProperty(type, textarea, Props.CareersText, "Carreiras — texto", "sections");
            AddProperty(type, textstring, Props.ContactTitle, "Contacto — título", "sections");
            AddProperty(type, textarea, Props.ContactText, "Contacto — texto", "sections");

            type.AddPropertyGroup("site", "Dados do site");
            AddProperty(type, textstring, Props.ContactEmail, "Email de contacto", "site", "Deixar vazio até estar aprovado.");
            AddProperty(type, textstring, Props.ContactPhone, "Telefone / WhatsApp", "site", "Deixar vazio até estar aprovado.");
            AddProperty(type, textarea, Props.ContactAddress, "Morada", "site", "Deixar vazio até estar aprovada para publicação.");
            AddProperty(type, textstring, Props.LegalName, "Denominação legal", "site");
            AddProperty(type, textarea, Props.FooterText, "Texto do rodapé", "site");
            AddProperty(type, textarea, Props.DemoNotice, "Aviso de demonstração (faixa no topo)", "site",
                "Apagar o texto para remover a faixa.");

            AddSeoAndDemo(type);
        });

        return new DocTypes(home, content, services, service, careers, contact);
    }

    private async Task<IContentType> CreatePageTypeAsync(string alias, string name, string icon, Action<ContentType> configure)
    {
        var template = await CreateTemplateAsync(alias, name);
        var type = NewType(alias, name, icon);
        configure(type);
        type.AllowedTemplates = [template];
        type.SetDefaultTemplate(template);
        await Expect(contentTypeService.CreateAsync(type, User), alias);
        return type;
    }

    private ContentType NewType(string alias, string name, string icon) => new(shortStringHelper, Constants.System.Root)
    {
        Alias = alias,
        Name = name,
        Icon = icon,
        Variations = ContentVariation.Culture,
    };

    private void AddProperty(ContentType type, IDataType dataType, string alias, string name, string group,
        string? description = null, bool variant = true)
    {
        var property = new PropertyType(shortStringHelper, dataType, alias)
        {
            Name = name,
            Description = description,
            Variations = variant ? ContentVariation.Culture : ContentVariation.Nothing,
        };
        type.AddPropertyType(property, group);
    }

    /// <summary>
    /// Regista o template no Umbraco reutilizando o ficheiro versionado em Views/{alias}.cshtml,
    /// para que o registo não substitua o ficheiro existente.
    /// </summary>
    private async Task<ITemplate> CreateTemplateAsync(string alias, string name)
    {
        if (await templateService.GetAsync(alias) is { } existing)
        {
            return existing;
        }

        var path = Path.Combine(environment.ContentRootPath, "Views", $"{alias}.cshtml");
        var markup = await System.IO.File.ReadAllTextAsync(path);
        var result = await templateService.CreateAsync(name, alias, markup, User);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Falha ao criar template {alias}: {result.Status}");
        }

        return result.Result;
    }

    private async Task CreateContentAsync(DocTypes types)
    {
        string[] cultures = [Pt, En];

        var home = contentService.Create("Project Energy", Constants.System.Root, types.Home);
        home.SetCultureName("Project Energy", Pt);
        home.SetCultureName("Project Energy", En);
        SetValues(home, DemoContent.Home);
        home.SetValue(Props.IsDemoContent, true);
        SaveAndPublish(home, cultures);

        foreach (var page in DemoContent.Pages)
        {
            var node = contentService.Create(page.Name.Pt, home, page.DocType);
            node.SetCultureName(page.Name.Pt, Pt);
            node.SetCultureName(page.Name.En, En);
            SetValues(node, page.Values);
            node.SetValue(Props.IsDemoContent, true);
            SaveAndPublish(node, cultures);

            if (page.DocType == ServicesPage)
            {
                foreach (var service in DemoContent.Services)
                {
                    var item = contentService.Create(service.Name.Pt, node, ServiceItem);
                    item.SetCultureName(service.Name.Pt, Pt);
                    item.SetCultureName(service.Name.En, En);
                    item.SetValue(Props.Summary, service.Summary.Pt, Pt);
                    item.SetValue(Props.Summary, service.Summary.En, En);
                    item.SetValue(Props.IsPlanned, service.IsPlanned);
                    item.SetValue(Props.Icon, service.Icon);
                    SaveAndPublish(item, cultures);
                }
            }
        }

        // URLs por idioma: /pt/... e /en/...
        var domains = await domainService.UpdateDomainsAsync(home.Key, new DomainsUpdateModel
        {
            DefaultIsoCode = Pt,
            Domains =
            [
                new DomainModel { DomainName = "/pt", IsoCode = Pt },
                new DomainModel { DomainName = "/en", IsoCode = En },
            ],
        });
        if (!domains.Success)
        {
            throw new InvalidOperationException($"Falha ao configurar domínios: {domains.Status}");
        }
    }

    private static void SetValues(IContent content, Dictionary<string, DemoContent.Text> values)
    {
        foreach (var (alias, text) in values)
        {
            content.SetValue(alias, Format(alias, text.Pt), Pt);
            content.SetValue(alias, Format(alias, text.En), En);
        }
    }

    // O editor de texto rico guarda JSON com o HTML em "markup".
    private static string Format(string alias, string value)
        => alias == Props.Body ? JsonSerializer.Serialize(new { markup = value, blocks = (object?)null }) : value;

    private void SaveAndPublish(IContent content, string[] cultures)
    {
        var result = contentService.SaveAndPublish(content, cultures);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Falha ao publicar '{content.Name}': {result.Result}");
        }
    }

    private static async Task Expect<TResult, TStatus>(Task<Attempt<TResult, TStatus>> operation, string what)
    {
        var attempt = await operation;
        if (!attempt.Success)
        {
            throw new InvalidOperationException($"Falha ao criar {what}: {attempt.Status}");
        }
    }

    private static async Task Expect<TStatus>(Task<Attempt<TStatus>> operation, string what)
    {
        var attempt = await operation;
        if (!attempt.Success)
        {
            throw new InvalidOperationException($"Falha ao criar {what}: {attempt.Result}");
        }
    }
}
