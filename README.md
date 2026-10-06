# Project Energy — Website

Website institucional da Project Energy, em ASP.NET Core MVC (C# + Razor Views) com Tailwind CSS.

> **Estado: fundação técnica com conteúdo de demonstração.**
> Todos os textos, serviços, certificações e contactos apresentados são fictícios e estão
> identificados como tal no website. Não representam dados reais da Project Energy.

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0) (LTS) — validado com 10.0.401
- [Node.js](https://nodejs.org/) 20 ou superior com npm — validado com Node 24 / npm 11 (apenas para compilar o CSS)

## Estrutura

```
ProjectEnergy.slnx
src/ProjectEnergy.Web/
  Controllers/HomeController.cs   Páginas institucionais
  Views/Home/*.pt.cshtml          Conteúdo em português
  Views/Home/*.en.cshtml          Conteúdo em inglês
  Views/Shared/                   Layout e componentes partilhados
  Resources/SharedResource.*.resx Textos partilhados (navegação, avisos, rodapé)
  Styles/site.css                 Fonte Tailwind (tokens de tema provisórios)
  wwwroot/css/site.css            CSS gerado — não versionado
docs/decisions/                   Registo de decisões técnicas
```

## Execução local

```bash
cd src/ProjectEnergy.Web
npm ci
dotnet run --launch-profile http
```

Abrir http://localhost:5134 (redireciona para `/pt`). Páginas disponíveis em `/pt/...` e `/en/...`:
`about`, `services`, `hseq`, `localcontent`, `careers`, `contact`.

## Compilação do CSS

O `dotnet build` executa automaticamente `npm run build:css` (requer `npm ci` prévio).

- Compilar manualmente: `npm run build:css`
- Recompilar ao editar views: `npm run watch:css` (em paralelo com `dotnet watch`)
- Compilar só o .NET: `dotnet build -p:SkipTailwindBuild=true`

## Ambientes

| Ambiente    | Configuração                     | Como executar                          |
|-------------|----------------------------------|----------------------------------------|
| Development | `appsettings.Development.json`   | `dotnet run --launch-profile http`     |
| Staging     | `appsettings.Staging.json`       | `dotnet run --launch-profile staging`  |
| Production  | `appsettings.Production.json`    | `ASPNETCORE_ENVIRONMENT=Production`    |

Nenhum ficheiro de configuração contém credenciais. Segredos futuros devem usar
[User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) em desenvolvimento
e variáveis de ambiente ou um cofre de segredos nos restantes ambientes.

## Limitações deste incremento

- Formulários de Contacto/RFQ e Carreiras são **apenas visuais**: não existe envio nem upload de CV.
- O website inclui `noindex, nofollow` enquanto o conteúdo for de demonstração.
- Sem CMS, base de dados, analytics, serviços externos ou deployment.

## Decisões pendentes

Os documentos "Website Requirements & Scope of Work", "Client Information Request" e as decisões
do Sprint 0 não estavam disponíveis no início deste trabalho. Ficam por decidir:

- **CMS** — se e qual. A estrutura MVC atual é provisória e sujeita a esta decisão.
- **Conteúdo real** — textos, serviços, certificações HSEQ, conteúdo local, vagas e contactos.
- **Idioma por omissão** e eventual adição de outros idiomas (atualmente `pt` e `en`, com `pt` por omissão).
- **Identidade visual** — logótipo, paleta e tipografia (os tokens em `Styles/site.css` são provisórios).
- **Formulários** — destino dos pedidos (email/CRM), proteção anti-spam, armazenamento de CVs e RGPD.
- **Alojamento e deployment**, domínio e valor de `AllowedHosts` em produção.
- **Analytics** e política de cookies.

Ver [docs/decisions/0001-initial-foundation.md](docs/decisions/0001-initial-foundation.md).
