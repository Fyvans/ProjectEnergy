# Estado do projeto

Atualizado a 2026-10-08. O backlog vive nas [issues do GitHub](https://github.com/Fyvans/ProjectEnergy/issues),
organizado por [milestones](https://github.com/Fyvans/ProjectEnergy/milestones) (sprints de 2 semanas).

## Onde estamos

**Fase A (lançamento) — Sprint 0 concluído; Sprint 1 a iniciar.** A base técnica, o CMS, a identidade
visual e as 7 páginas bilingues existem com conteúdo de demonstração. O lançamento está bloqueado por
conteúdo aprovado, dados da empresa e a decisão de alojamento — todos do lado do cliente.

| Sprint | Período | Objetivo | Estado |
|---|---|---|---|
| [Sprint 0 — Fundação](https://github.com/Fyvans/ProjectEnergy/milestone/1?closed=1) | até 9 out. | Base técnica, CMS, marca, pré-visualização | Concluído (5/5) |
| [Sprint 1 — Revisão e bases técnicas](https://github.com/Fyvans/ProjectEnergy/milestone/2) | 12–23 out. | Feedback do cliente, pipeline de pré-visualização no Git, SEO base, decisões | Planeado (0/8) |
| [Sprint 2 — Formulários e ambientes](https://github.com/Fyvans/ProjectEnergy/milestone/3) | 26 out.–6 nov. | Formulários reais, privacidade, staging, CI/CD | Planeado (0/6) |
| [Sprint 3 — Conteúdo final e lançamento](https://github.com/Fyvans/ProjectEnergy/milestone/4) | 9–20 nov. | Conteúdo aprovado, analytics, QA, produção, entrega | Planeado (0/6) |
| [Fase B — Maturidade comercial](https://github.com/Fyvans/ProjectEnergy/milestone/5) | após lançamento | Projetos, certificações, downloads | Backlog (0/3) |

As datas dos sprints são uma proposta; o Sprint 3 só fecha se os bloqueios do cliente forem resolvidos
até ao fim do Sprint 1.

## Revisão do Sprint 0

Entregue:

- Solution .NET 10 LTS com Umbraco 17 LTS, Tailwind e configuração por ambiente (#1, #2).
- Modelo de conteúdo editável PT/EN: página inicial, conteúdo, serviços (disponível/planeado), carreiras, contacto (#2).
- Identidade visual derivada do logótipo; layout responsivo (#3).
- Pré-visualização protegida por palavra-passe e backoffice oculto (#4).
- Análise de opções de alojamento e custos (#5).
- Pré-visualização estática publicada em <https://projectenergy-preview.pages.dev> a partir do Mac mini.

Não entregue / dívida técnica:

- O processo de exportação estática e deploy para Cloudflare Pages não está no repositório (#6).
- Formulários apenas visuais (planeados no Sprint 2).
- `canonical`/`hreflang` relativos na versão estática (#11).

## Impedimentos (dependem do cliente)

| Impedimento | Issue | Bloqueia |
|---|---|---|
| Textos aprovados PT/EN e lista de serviços oferecidos vs. planeados | #8 | #20, lançamento |
| Denominação legal, NIF e contactos | #8 | Rodapé, contacto, #20 |
| Logótipo em SVG / versão horizontal | #8 | Acabamento visual |
| Parecer sobre localização de dados pessoais | #9 | Alojamento, #15, #18, #23 |
| Registo do domínio | #10 | Endereço final, email, #12, #23 |
| Texto da política de privacidade | #16 | Formulários em produção |

## Sprint 1 — compromisso

Prioridade alta: #6 (pipeline de pré-visualização no Git), #7 (revisão com o cliente), #8 (pedido de conteúdos),
#9 (decisão de alojamento). A seguir: #11 (SEO base), #12 (acesso à pré-visualização), #10 (domínios), #13 (branches).

## Definição de pronto

Uma issue só fecha quando:

1. Os critérios de aceitação estão cumpridos.
2. A solução compila e o site corre localmente sem erros.
3. A alteração foi verificada em PT e EN e em mobile.
4. Não há segredos, dados pessoais ou conteúdo confidencial no repositório (o repositório é público).
5. O código está na `main` através de commit ou PR com mensagem clara, e a documentação foi atualizada se necessário.
