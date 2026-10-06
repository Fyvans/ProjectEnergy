# 0001 — Fundação técnica inicial

- **Data:** 2026-10-06
- **Estado:** Revista pela [0002](0002-umbraco-cms-and-brand.md) (decisões 1, 4 e 7 substituídas)

## Contexto

Os documentos "Project Energy — Website Requirements & Scope of Work", "Client Information Request"
e as decisões aprovadas do Sprint 0 não estavam disponíveis quando este incremento foi iniciado.
Não existia `AGENTS.md` no repositório. Por isso, nenhuma decisão de CMS ou conteúdo foi assumida.

## Decisões

1. **Um único projeto ASP.NET Core MVC** (`src/ProjectEnergy.Web`) numa solution `.slnx`.
   Sem projetos adicionais, base de dados, APIs ou camadas de repositório — não há requisito que os justifique.
   *Provisório:* se for aprovado um CMS, esta estrutura pode ter de ser revista.
2. **.NET 10 (LTS)**, suportado até novembro de 2028. A compatibilidade com um CMS será verificada quando este for escolhido.
3. **Tailwind CSS 4.3.3** via `@tailwindcss/cli`, com versões fixadas em `package.json` e `package-lock.json`.
   Compilação local integrada no `dotnet build`; o CSS gerado não é versionado. Sem CDN nem fontes externas.
4. **Idiomas PT/EN por prefixo de URL** (`/pt/...`, `/en/...`) com `RouteDataRequestCultureProvider`.
   O conteúdo das páginas está em views por idioma (`About.pt.cshtml`, `About.en.cshtml`) e os textos
   partilhados em `Resources/SharedResource.*.resx`. `pt` é o idioma por omissão provisório.
5. **Conteúdo de demonstração explicitamente identificado**: faixa global, etiquetas "Demonstração/Demo",
   marcadores `[Exemplo]` e notas. Nenhum serviço, certificação, projeto, número ou contacto é apresentado como real.
   Meta `noindex, nofollow` enquanto o conteúdo for de demonstração.
6. **Formulários apenas visuais**: campos dentro de `<fieldset disabled>`, sem elemento `<form>`, sem endpoint
   e sem campo de upload. Implementação real depende de decisões sobre destino, anti-spam e RGPD.
7. **Configuração por ambiente** (Development, Staging, Production) apenas com níveis de logging;
   sem credenciais. Logging padrão do ASP.NET Core.
8. **Sem testes automáticos neste incremento**: não há lógica de negócio; a validação foi feita por
   compilação e verificação manual das rotas.

## Consequências

- Substituir conteúdo de demonstração exige editar as views por idioma (ou migrar para CMS).
- Contribuidores precisam de Node.js para compilar o CSS.
