using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using ProjectEnergy.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    // Páginas de conteúdo por idioma: Views/Home/About.pt.cshtml, About.en.cshtml.
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(SiteCultures.Default)
        .AddSupportedCultures(SiteCultures.Supported)
        .AddSupportedUICultures(SiteCultures.Supported);

    // O idioma é determinado apenas pelo prefixo do URL (/pt/..., /en/...).
    options.RequestCultureProviders =
    [
        new RouteDataRequestCultureProvider { RouteDataStringKey = "culture", UIRouteDataStringKey = "culture" }
    ];
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler($"/{SiteCultures.Default}/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseRequestLocalization();

app.UseAuthorization();

app.MapStaticAssets();

app.MapGet("/", () => Results.Redirect($"/{SiteCultures.Default}"));

app.MapControllerRoute(
    name: "localized",
    pattern: "{culture:regex(^(pt|en)$)}/{action=Index}",
    defaults: new { controller = "Home" })
    .WithStaticAssets();

app.Run();
