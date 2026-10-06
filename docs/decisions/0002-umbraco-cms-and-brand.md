# 0002 — Umbraco CMS e identidade visual

- **Data:** 2026-10-06
- **Estado:** Aceite (CMS escolhido pelo responsável do projeto); conteúdo e marca continuam provisórios
- **Revê:** [0001](0001-initial-foundation.md), decisões 1, 4 e 7

## Contexto

O documento "Project Energy — Website Requirements & Scope of Work" (v1.0, setembro 2026, confidencial —
não versionado neste repositório) exige:

- §8: um administrador não técnico tem de editar textos, imagens, serviços e vagas;
- §12: "WordPress ou CMS mainstream equivalente com suporte a longo prazo", evitando CMS feito à medida;
- §4/§8: versões EN e PT equivalentes, com seletor de idioma no cabeçalho;
- §20: navegação HOME | ABOUT | SERVICES | HSEQ | LOCAL CONTENT | CAREERS | CONTACT.

A fundação MVC da decisão 0001 não cumpria §8 e §12. Foi também fornecido o logótipo definitivo.

## Decisões

1. **Umbraco CMS 17 LTS (17.7.1)** sobre .NET 10 LTS — CMS open-source mainstream em ASP.NET Core,
   suportado até novembro de 2028. Mantém C#, Razor e Tailwind. Versões geridas em `Directory.Packages.props`.
   O Umbraco 18 (STS) foi preterido por ter suporte mais curto.
2. **Base de dados:** SQLite em Development (`umbraco/Data`, não versionado). Staging e Production
   requerem SQL Server através de `ConnectionStrings__umbracoDbDSN` — pendente de decisão de alojamento.
3. **Modelo de conteúdo criado em código** (`Content/DemoContentSeeder.cs`) apenas numa base de dados vazia:
   idiomas `pt` (por omissão) e `en`, dicionário de textos de interface, tipos de documento
   (`homePage`, `contentPage`, `servicesPage`, `serviceItem`, `careersPage`, `contactPage`), templates e
   conteúdo de demonstração. Depois disso o conteúdo é gerido apenas no backoffice e nunca é sobrescrito.
4. **Idiomas por domínio relativo** `/pt` e `/en`, com URLs legíveis derivados do nome de cada página
   (`/pt/sobre-nos/`, `/en/about-us/`). A raiz `/` redireciona para `/pt/`.
5. **Serviços como itens editáveis** com indicação "disponível" / "capacidade planeada" (§6 dos requisitos).
   A classificação atual é ilustrativa e tem de ser confirmada pelo cliente.
6. **Conteúdo de demonstração** segue a estrutura e o posicionamento recomendados nos requisitos (§3, §5–§7),
   sem clientes, logótipos de operadoras, certificações, estatísticas, moradas ou contactos.
   Continua identificado por faixa global (editável) e por etiqueta "Demonstração" por página
   (campo "Conteúdo de demonstração"). Meta `noindex, nofollow` mantém-se.
7. **Identidade visual:** paleta derivada do logótipo (azul-marinho, verde, laranja), tipografia
   Montserrat (títulos) e Inter (texto) servidas localmente (SIL OFL 1.1), ícones Heroicons (MIT).
   O logótipo foi convertido para WebP com fundo transparente; falta versão horizontal e vetorial oficial.
8. **Telemetria Umbraco** definida para o nível mínimo pelo seeder até haver decisão sobre analytics.
9. **Credenciais do administrador local** apenas em `dotnet user-secrets` (instalação unattended em Development).

## Consequências

- Templates (`Views/*.cshtml`) continuam versionados; o Umbraco também os pode editar no backoffice —
  alterações feitas no backoffice em ambientes partilhados têm de ser trazidas para o repositório.
- Alterações futuras ao modelo de conteúdo em bases de dados já existentes exigem migração própria
  (ou uma ferramenta como uSync) — a decidir antes do primeiro ambiente partilhado.
- A ligação entre a página inicial e as páginas HSEQ / Conteúdo Local depende da ordem das páginas na árvore.
