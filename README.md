# Project Energy — Website

Website institucional da Project Energy em **Umbraco CMS 17 LTS** (.NET 10, C#, Razor) com Tailwind CSS.

> **Estado: fundação técnica com conteúdo de demonstração.**
> Os textos seguem a estrutura dos requisitos do cliente mas **não foram aprovados**. Serviços,
> certificações e contactos não estão confirmados; o website identifica-o em todas as páginas.

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0) (LTS) — validado com 10.0.401
- [Node.js](https://nodejs.org/) 20+ com npm — validado com Node 24 / npm 11 (apenas para compilar o CSS)

## Estrutura

```
ProjectEnergy.slnx
Directory.Packages.props               Versões NuGet (Umbraco 17.7.1)
src/ProjectEnergy.Web/
  Content/DemoContentSeeder.cs         Cria modelo e conteúdo de demonstração numa BD vazia
  Content/DemoContent.cs               Textos de demonstração PT/EN
  Content/ContentAliases.cs            Aliases de tipos de documento e propriedades
  Content/SiteIcons.cs                 Ícones SVG (Heroicons, MIT)
  Views/{homePage,contentPage,...}.cshtml  Templates Umbraco
  Views/Shared/_Layout.cshtml          Layout (cabeçalho, navegação, rodapé)
  Views/Partials/                      Componentes partilhados
  Styles/site.css                      Fonte Tailwind (design system)
  wwwroot/fonts, wwwroot/images        Fontes (OFL) e logótipo
  wwwroot/css/site.css                 CSS gerado — não versionado
  umbraco/Data, umbraco/Logs           Base de dados SQLite e logs locais — não versionados
docs/decisions/                        Registo de decisões técnicas
```

## Execução local

1. Instalar dependências do CSS:

   ```bash
   cd src/ProjectEnergy.Web
   npm ci
   ```

2. Definir as credenciais do administrador local (apenas na primeira execução; ficam fora do repositório):

   ```bash
   dotnet user-secrets set "Umbraco:CMS:Unattended:UnattendedUserName" "Administrador Local"
   dotnet user-secrets set "Umbraco:CMS:Unattended:UnattendedUserEmail" "admin@projectenergy.test"
   dotnet user-secrets set "Umbraco:CMS:Unattended:UnattendedUserPassword" "<palavra-passe forte>"
   ```

3. Executar:

   ```bash
   dotnet run --launch-profile http
   ```

Na primeira execução o Umbraco instala-se em SQLite e cria o modelo e o conteúdo de demonstração.

- Website: http://localhost:5134 (redireciona para `/pt/`; inglês em `/en/`)
- Backoffice: http://localhost:5134/umbraco (credenciais definidas no passo 2)

Para recomeçar do zero, parar a aplicação e apagar `src/ProjectEnergy.Web/umbraco/Data`.

## Edição de conteúdo

No backoffice, em **Content**: página inicial (destaque, secções, dados do site, faixa de demonstração),
páginas internas e itens de serviço. Textos de interface (botões, etiquetas de formulário) estão em
**Translation**. Para remover a etiqueta "Demonstração" de uma página, desmarcar
"Conteúdo de demonstração" no separador *Estado*; para remover a faixa global, apagar o
"Aviso de demonstração" na página inicial.

## Compilação do CSS

O `dotnet build` executa automaticamente `npm run build:css` (requer `npm ci` prévio).

- Compilar manualmente: `npm run build:css`
- Recompilar ao editar templates: `npm run watch:css`
- Compilar só o .NET: `dotnet build -p:SkipTailwindBuild=true`

## Ambientes

| Ambiente    | Base de dados                       | Configuração                   |
|-------------|-------------------------------------|--------------------------------|
| Development | SQLite local (instalação automática) | `appsettings.Development.json` |
| Staging     | SQL Server (a definir)               | `appsettings.Staging.json` + `ConnectionStrings__umbracoDbDSN` |
| Production  | SQL Server (a definir)               | `appsettings.Production.json` + `ConnectionStrings__umbracoDbDSN` |

Nenhum ficheiro versionado contém credenciais ou connection strings de servidores.

## Limitações deste incremento

- Formulários de Contacto/RFQ e Carreiras são **apenas visuais**: sem envio, sem upload de CV.
- `noindex, nofollow` em todas as páginas enquanto o conteúdo for de demonstração.
- Sem analytics, cookies, mapas, sitemap ou serviços externos.

## Decisões pendentes

- **Conteúdo aprovado** em PT e EN (empresa, missão, serviços oferecidos vs. planeados, HSEQ, conteúdo local).
- **Dados da empresa:** denominação legal, NIF, morada, email, telefone/WhatsApp, horário.
- **Marca:** versão horizontal e vetorial (SVG) do logótipo, cores oficiais, guidelines.
- **Idioma por omissão** (atualmente `pt`).
- **Formulários:** destino (email/CRM), anti-spam, armazenamento seguro de CVs, retenção, política de privacidade.
- **Alojamento** (SQL Server, backups diários, SSL), domínio (.com/.ao), email corporativo, deployment.
- **Gestão de alterações ao modelo de conteúdo** entre ambientes (migrações ou uSync).
- **Analytics** (Google Analytics ou alternativa), Search Console e consentimento de cookies.
- Fotografia original ou licenciada.

Ver [docs/decisions](docs/decisions).
