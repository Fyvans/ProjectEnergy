using static ProjectEnergy.Web.Content.ContentAliases;

namespace ProjectEnergy.Web.Content;

/// <summary>
/// Conteúdo de DEMONSTRAÇÃO usado apenas para popular uma base de dados vazia.
/// Estrutura e posicionamento seguem o documento de requisitos do cliente; nenhum texto foi aprovado
/// pela Project Energy. Não inclui clientes, certificações, estatísticas nem contactos reais.
/// Depois da primeira execução, o conteúdo é gerido exclusivamente no backoffice Umbraco.
/// </summary>
public static class DemoContent
{
    public sealed record Text(string Pt, string En);

    public sealed record Page(string DocType, Text Name, Dictionary<string, Text> Values);

    public sealed record Service(Text Name, Text Summary, string Icon, bool IsPlanned);

    /// <summary>Textos de interface (Dicionário Umbraco), editáveis em Tradução no backoffice.</summary>
    public static readonly Dictionary<string, Text> Dictionary = new()
    {
        ["Nav.SkipToContent"] = new("Saltar para o conteúdo", "Skip to content"),
        ["Nav.Main"] = new("Navegação principal", "Main navigation"),
        ["Nav.Menu"] = new("Menu", "Menu"),
        ["Nav.Language"] = new("Idioma", "Language"),
        ["Demo.Badge"] = new("Demonstração", "Demo"),
        ["Cta.RequestQuote"] = new("Pedir cotação", "Request a Quote"),
        ["Cta.TalkToTeam"] = new("Fale com a nossa equipa", "Talk to our team"),
        ["Cta.LearnMore"] = new("Saber mais", "Learn more"),
        ["Cta.AllServices"] = new("Ver todos os serviços", "View all services"),
        ["Cta.JoinTalentPool"] = new("Registar na bolsa de talentos", "Join our talent pool"),
        ["Home.HeroEyebrow"] = new("Oil & Gas · Energia · Indústria", "Oil & Gas · Energy · Industry"),
        ["Home.ServicesEyebrow"] = new("Serviços", "Services"),
        ["Home.ServicesTitle"] = new("Capacidades para operações exigentes", "Capabilities for demanding operations"),
        ["Home.AboutEyebrow"] = new("Quem somos", "Who we are"),
        ["Home.HseqEyebrow"] = new("HSEQ", "HSEQ"),
        ["Home.LocalEyebrow"] = new("Conteúdo local", "Local content"),
        ["Home.CareersEyebrow"] = new("Carreiras", "Careers"),
        ["Home.ContactEyebrow"] = new("Contacto", "Contact"),
        ["Service.Available"] = new("Disponível", "Available"),
        ["Service.Planned"] = new("Capacidade planeada", "Planned capability"),
        ["Service.Pending"] = new("Âmbito sujeito a confirmação pela Project Energy.", "Scope subject to confirmation by Project Energy."),
        ["Contact.Email"] = new("Email", "Email"),
        ["Contact.Phone"] = new("Telefone / WhatsApp", "Phone / WhatsApp"),
        ["Contact.Address"] = new("Morada", "Address"),
        ["Contact.Hours"] = new("Horário", "Business hours"),
        ["Contact.ToConfirm"] = new("A confirmar", "To be confirmed"),
        ["Form.Name"] = new("Nome", "Name"),
        ["Form.Company"] = new("Empresa", "Company"),
        ["Form.Email"] = new("Email", "Email"),
        ["Form.Phone"] = new("Telefone", "Phone"),
        ["Form.EnquiryType"] = new("Tipo de pedido", "Enquiry type"),
        ["Form.EnquiryGeneral"] = new("Pedido geral", "General enquiry"),
        ["Form.EnquiryQuote"] = new("Pedido de cotação (RFQ)", "Request for quotation (RFQ)"),
        ["Form.EnquiryPartnership"] = new("Parceria / subcontratação", "Partnership / subcontracting"),
        ["Form.Message"] = new("Mensagem", "Message"),
        ["Form.Profession"] = new("Profissão", "Profession"),
        ["Form.Experience"] = new("Anos de experiência", "Years of experience"),
        ["Form.Location"] = new("Localização", "Location"),
        ["Form.Cv"] = new("CV", "CV"),
        ["Form.CvUnavailable"] = new("Carregamento de CV indisponível nesta versão", "CV upload unavailable in this version"),
        ["Form.Consent"] = new("Autorizo o tratamento dos meus dados para responder a este pedido, nos termos da política de privacidade.", "I consent to my data being processed to respond to this request, under the privacy notice."),
        ["Form.ConsentCv"] = new("Autorizo a conservação dos meus dados na bolsa de talentos, nos termos da política de privacidade.", "I consent to my data being retained in the talent pool, under the privacy notice."),
        ["Form.Send"] = new("Enviar pedido", "Send enquiry"),
        ["Form.SendApplication"] = new("Enviar candidatura", "Submit application"),
        ["Form.Disabled"] = new("Demonstração: este formulário ainda não envia dados.", "Demo: this form does not send any data yet."),
        ["Page.Home"] = new("Início", "Home"),
        ["Footer.Pages"] = new("Navegação", "Navigation"),
        ["Footer.Contact"] = new("Contacto", "Contact"),
        ["Footer.Rights"] = new("Todos os direitos reservados.", "All rights reserved."),
    };

    public static readonly Dictionary<string, Text> Home = new()
    {
        [Props.HeroTitle] = new(
            "Engenharia, fabricação e serviços industriais para o setor energético de Angola",
            "Engineering, Fabrication & Industrial Services for Angola's Energy Sector"),
        [Props.HeroText] = new(
            "Apoiamos a indústria energética angolana com pessoas qualificadas, qualidade, segurança e capacidade local.",
            "Supporting Angola's energy industry with skilled people, quality, safety and local capability."),
        [Props.IntroTitle] = new(
            "Um parceiro local, focado em segurança e competência técnica",
            "A local partner focused on safety and technical competence"),
        [Props.IntroText] = new(
            "A Project Energy fornece mão de obra técnica e serviços industriais a operadores, empreiteiros EPC e empresas de serviços petrolíferos em Angola. Crescemos de forma progressiva, ativando novas capacidades apenas quando as pessoas, as qualificações e as instalações estão prontas.",
            "Project Energy provides technical manpower and industrial services to operators, EPC contractors and oilfield service companies in Angola. We grow progressively, activating new capabilities only when the people, qualifications and facilities are in place."),
        [Props.HseqTitle] = new(
            "Segurança e qualidade em cada trabalho",
            "Safety and quality in every job"),
        [Props.HseqText] = new(
            "Saúde, segurança, ambiente e qualidade orientam a forma como planeamos, mobilizamos e executamos. As políticas e certificações serão publicadas apenas quando aprovadas e válidas.",
            "Health, safety, environment and quality guide how we plan, mobilise and execute. Policies and certifications will only be published once approved and valid."),
        [Props.LocalContentTitle] = new(
            "Compromisso com o conteúdo local",
            "Committed to local content"),
        [Props.LocalContentText] = new(
            "Desenvolvimento da força de trabalho angolana, formação, transferência de conhecimento e cadeias de fornecimento locais estão no centro da nossa estratégia.",
            "Angolan workforce development, training, knowledge transfer and local supply chains are at the heart of our strategy."),
        [Props.CareersTitle] = new(
            "Construa a sua carreira connosco",
            "Build your career with us"),
        [Props.CareersText] = new(
            "Soldadores, serralheiros de tubagem, fabricantes, riggers, pintores industriais, técnicos GRE/GRP, profissionais de HSE e outros especialistas: registe-se na nossa bolsa de talentos.",
            "Welders, pipe fitters, fabricators, riggers, industrial painters, GRE/GRP technicians, HSE personnel and other specialists: join our talent pool."),
        [Props.ContactTitle] = new(
            "Vamos falar sobre o seu próximo projeto",
            "Let's talk about your next project"),
        [Props.ContactText] = new(
            "Pedidos de cotação, necessidades de mão de obra, oportunidades de subcontratação ou questões gerais.",
            "Requests for quotation, manpower requirements, subcontracting opportunities or general enquiries."),
        [Props.ContactEmail] = new("", ""),
        [Props.ContactPhone] = new("", ""),
        [Props.ContactAddress] = new("", ""),
        [Props.DemoNotice] = new(
            "Versão de demonstração — textos provisórios, sujeitos a aprovação pela Project Energy. Serviços, certificações e contactos ainda não confirmados.",
            "Demo version — provisional text pending Project Energy approval. Services, certifications and contacts not yet confirmed."),
        [Props.LegalName] = new("Project Energy", "Project Energy"),
        [Props.FooterText] = new(
            "Engenharia, fabricação e serviços industriais para o setor energético de Angola.",
            "Engineering, fabrication and industrial services for Angola's energy sector."),
        [Props.MetaDescription] = new(
            "Project Energy — mão de obra técnica, manutenção, soldadura, fabricação e serviços industriais para o setor de Oil & Gas em Angola.",
            "Project Energy — technical manpower, maintenance, welding, fabrication and industrial services for Angola's Oil & Gas sector."),
    };

    public static readonly Page[] Pages =
    [
        new(ContentPage, new("Sobre Nós", "About Us"), new()
        {
            [Props.Intro] = new(
                "Uma empresa em crescimento, dedicada a apoiar o setor energético angolano com pessoas qualificadas e uma cultura de segurança.",
                "A growing company supporting Angola's energy sector with qualified people and a culture of safety."),
            [Props.Body] = new(
                "<h2>Visão geral</h2><p>A Project Energy apoia operadores, empreiteiros EPC/EPCI e empresas de serviços com mão de obra técnica e serviços industriais, com o objetivo de evoluir para fabricação e serviços industriais integrados.</p><h2>Missão</h2><p>Fornecer pessoas competentes e serviços seguros, com qualidade consistente e respeito pelas comunidades onde trabalhamos.</p><h2>Visão</h2><p>Ser uma referência angolana em serviços industriais para o setor energético, reconhecida pela segurança, pela competência técnica e pelo desenvolvimento de talento local.</p><h2>Valores</h2><ul><li>Segurança acima de tudo</li><li>Integridade e factualidade</li><li>Qualidade técnica</li><li>Desenvolvimento das pessoas</li><li>Compromisso com Angola</li></ul><h2>Estratégia de crescimento</h2><p>As novas capacidades — incluindo fabricação para Oil &amp; Gas — serão ativadas de forma progressiva, apenas quando as qualificações, as instalações e as aprovações estiverem asseguradas.</p>",
                "<h2>Overview</h2><p>Project Energy supports operators, EPC/EPCI contractors and service companies with technical manpower and industrial services, with the aim of growing into fabrication and integrated industrial services.</p><h2>Mission</h2><p>To provide competent people and safe services, with consistent quality and respect for the communities where we work.</p><h2>Vision</h2><p>To become an Angolan reference in industrial services for the energy sector, recognised for safety, technical competence and local talent development.</p><h2>Values</h2><ul><li>Safety first</li><li>Integrity and factual communication</li><li>Technical quality</li><li>People development</li><li>Commitment to Angola</li></ul><h2>Growth strategy</h2><p>New capabilities — including Oil &amp; Gas fabrication — will be activated progressively, only once qualifications, facilities and approvals are in place.</p>"),
            [Props.MetaDescription] = new(
                "Conheça a Project Energy: missão, visão, valores e estratégia de crescimento no setor energético de Angola.",
                "About Project Energy: mission, vision, values and growth strategy in Angola's energy sector."),
        }),
        new(ServicesPage, new("Serviços", "Services"), new()
        {
            [Props.Intro] = new(
                "Serviços técnicos e industriais para Oil & Gas e indústria, ativados de forma progressiva à medida que a empresa cresce.",
                "Technical and industrial services for Oil & Gas and industry, activated progressively as the company grows."),
            [Props.Body] = new(
                "<p>As capacidades marcadas como planeadas só serão oferecidas quando as qualificações, instalações e aprovações necessárias estiverem asseguradas.</p>",
                "<p>Capabilities marked as planned will only be offered once the required qualifications, facilities and approvals are in place.</p>"),
            [Props.MetaDescription] = new(
                "Mão de obra técnica, manutenção, soldadura e tubagem, fabricação metálica, GRE/GRP/FRP, pintura industrial e pessoal HSE em Angola.",
                "Technical manpower, maintenance, welding and piping, metal fabrication, GRE/GRP/FRP, industrial coating and HSE personnel in Angola."),
        }),
        new(ContentPage, new("HSEQ", "HSEQ"), new()
        {
            [Props.Intro] = new(
                "Saúde, Segurança, Ambiente e Qualidade são a base de tudo o que fazemos.",
                "Health, Safety, Environment and Quality are the foundation of everything we do."),
            [Props.Body] = new(
                "<h2>O nosso compromisso</h2><p>Planeamos e executamos o trabalho para proteger as pessoas, o ambiente e os ativos dos nossos clientes, cumprindo os requisitos legais e contratuais aplicáveis.</p><h2>Abordagem de gestão</h2><ul><li>Avaliação de riscos antes da mobilização</li><li>Supervisão competente no terreno</li><li>Comunicação e reporte de incidentes e quase-acidentes</li><li>Melhoria contínua baseada em lições aprendidas</li></ul><h2>Competência e formação</h2><p>Os profissionais são selecionados e acompanhados de acordo com as qualificações exigidas para cada função.</p><h2>Certificações</h2><p>As certificações serão publicadas nesta página apenas depois de obtidas e enquanto se mantiverem válidas.</p>",
                "<h2>Our commitment</h2><p>We plan and execute work to protect people, the environment and our clients' assets, in compliance with applicable legal and contractual requirements.</p><h2>Management approach</h2><ul><li>Risk assessment before mobilisation</li><li>Competent supervision in the field</li><li>Incident and near-miss communication and reporting</li><li>Continuous improvement based on lessons learned</li></ul><h2>Competence and training</h2><p>Personnel are selected and managed according to the qualifications required for each role.</p><h2>Certifications</h2><p>Certifications will only be published on this page once obtained and while they remain valid.</p>"),
            [Props.MetaDescription] = new(
                "Compromisso HSEQ da Project Energy: saúde, segurança, ambiente e qualidade.",
                "Project Energy HSEQ commitment: health, safety, environment and quality."),
        }),
        new(ContentPage, new("Conteúdo Local", "Local Content"), new()
        {
            [Props.Intro] = new(
                "Desenvolver talento angolano e fortalecer a cadeia de fornecimento local.",
                "Developing Angolan talent and strengthening the local supply chain."),
            [Props.Body] = new(
                "<h2>Desenvolvimento da força de trabalho</h2><p>Priorizamos o recrutamento e a progressão de profissionais angolanos em funções técnicas e de supervisão.</p><h2>Formação e transferência de conhecimento</h2><p>Acompanhamento no terreno e formação estruturada para elevar competências técnicas e de segurança.</p><h2>Fornecedores locais</h2><p>Sempre que possível, trabalhamos com fornecedores e subcontratados angolanos qualificados.</p><h2>Capacidade futura</h2><p>A estratégia de crescimento inclui o desenvolvimento progressivo de capacidade de fabricação em Angola.</p>",
                "<h2>Workforce development</h2><p>We prioritise recruiting and advancing Angolan professionals in technical and supervisory roles.</p><h2>Training and knowledge transfer</h2><p>On-the-job coaching and structured training to raise technical and safety competence.</p><h2>Local suppliers</h2><p>Wherever possible, we work with qualified Angolan suppliers and subcontractors.</p><h2>Future capability</h2><p>Our growth strategy includes progressively developing fabrication capability in Angola.</p>"),
            [Props.MetaDescription] = new(
                "Conteúdo local na Project Energy: força de trabalho angolana, formação e fornecedores locais.",
                "Local content at Project Energy: Angolan workforce, training and local suppliers."),
        }),
        new(CareersPage, new("Carreiras", "Careers"), new()
        {
            [Props.Intro] = new(
                "Procuramos profissionais técnicos qualificados para projetos no setor energético e industrial em Angola.",
                "We are looking for qualified technical professionals for energy and industrial projects in Angola."),
            [Props.Body] = new(
                "<h2>Categorias profissionais</h2><ul><li>Soldadores e serralheiros de tubagem</li><li>Fabricantes e riggers</li><li>Pintores industriais</li><li>Técnicos GRE/GRP</li><li>Técnicos e supervisores de HSE</li><li>QA/QC e supervisão técnica</li></ul><p>De momento não existem vagas publicadas. Registe-se na bolsa de talentos para ser contactado quando surgirem oportunidades compatíveis.</p>",
                "<h2>Job categories</h2><ul><li>Welders and pipe fitters</li><li>Fabricators and riggers</li><li>Industrial painters</li><li>GRE/GRP technicians</li><li>HSE officers and supervisors</li><li>QA/QC and technical supervision</li></ul><p>There are no published vacancies at the moment. Join our talent pool to be contacted when suitable opportunities arise.</p>"),
            [Props.MetaDescription] = new(
                "Carreiras na Project Energy: bolsa de talentos para soldadores, serralheiros de tubagem, técnicos GRE/GRP e HSE em Angola.",
                "Careers at Project Energy: talent pool for welders, pipe fitters, GRE/GRP and HSE technicians in Angola."),
        }),
        new(ContactPage, new("Contacto", "Contact"), new()
        {
            [Props.Intro] = new(
                "Fale com a nossa equipa sobre pedidos de cotação, necessidades de mão de obra ou parcerias.",
                "Talk to our team about quotations, manpower requirements or partnerships."),
            [Props.Body] = new("", ""),
            [Props.MetaDescription] = new(
                "Contacte a Project Energy para pedidos de cotação e serviços industriais em Angola.",
                "Contact Project Energy for quotations and industrial services in Angola."),
        }),
    ];

    public static readonly Service[] Services =
    [
        new(new("Mão de obra técnica e outsourcing", "Technical manpower & outsourcing"),
            new("Soldadores, serralheiros de tubagem, fabricantes, riggers, pintores, técnicos GRE/GRP, profissionais de HSE, supervisores e QA/QC.",
                "Welders, pipe fitters, fabricators, riggers, painters, GRE/GRP technicians, HSE officers, supervisors and QA/QC."),
            "workers", false),
        new(new("Manutenção e apoio de campo", "Maintenance & field support"),
            new("Manutenção industrial, apoio a paragens e turnarounds e suporte técnico em instalações do cliente.",
                "Industrial maintenance, shutdown and turnaround manpower support, and client-site technical support."),
            "maintenance", false),
        new(new("Soldadura e tubagem", "Welding & piping"),
            new("Soldadura, fit-up, fabricação e montagem de tubagem, à medida que qualificações WPS/PQR/WPQ estejam disponíveis.",
                "Welding, fit-up, pipe fabrication and installation, as WPS/PQR/WPQ qualifications become available."),
            "welding", false),
        new(new("Fabricação metálica", "Metal fabrication"),
            new("Estruturas metálicas e fabricação ligeira numa fase inicial, com expansão futura para fabricação Oil & Gas.",
                "Structural steel and light fabrication initially, with future expansion into Oil & Gas fabrication."),
            "fabrication", false),
        new(new("GRE / GRP / FRP", "GRE / GRP / FRP"),
            new("Pessoal qualificado para colagem e instalação e, futuramente, fabricação ou reparação em oficina ou campo.",
                "Qualified bonding and installation personnel and, later, workshop or field fabrication and repair."),
            "composites", false),
        new(new("Pintura e revestimentos", "Painting & coating"),
            new("Pintura industrial, preparação de superfícies e revestimentos de proteção, com instalações e controlos ambientais adequados.",
                "Industrial painting, surface preparation and protective coating, with suitable facilities and environmental controls."),
            "coating", true),
        new(new("Pessoal HSE", "HSE personnel"),
            new("Fornecimento de profissionais de HSE qualificados para apoio a operações e projetos.",
                "Provision of qualified HSE personnel to support operations and projects."),
            "safety", false),
        new(new("Formação e competências", "Training & competence"),
            new("Futura capacidade de testes técnicos e formação profissional, ativada quando infraestrutura e aprovações estiverem asseguradas.",
                "Future technical trade-testing and training capability, activated once infrastructure and approvals are in place."),
            "training", true),
    ];
}
