namespace ProjectEnergy.Web.Content;

/// <summary>
/// Aliases do modelo de conteúdo Umbraco usados pelo seeder e pelos templates.
/// </summary>
public static class ContentAliases
{
    public const string HomePage = "homePage";
    public const string ContentPage = "contentPage";
    public const string ServicesPage = "servicesPage";
    public const string ServiceItem = "serviceItem";
    public const string CareersPage = "careersPage";
    public const string ContactPage = "contactPage";

    public static class Props
    {
        // Comuns às páginas
        public const string Intro = "intro";
        public const string Body = "body";
        public const string MetaDescription = "metaDescription";
        public const string IsDemoContent = "isDemoContent";

        // Página inicial
        public const string HeroTitle = "heroTitle";
        public const string HeroText = "heroText";
        public const string IntroTitle = "introTitle";
        public const string IntroText = "introText";
        public const string HseqTitle = "hseqTitle";
        public const string HseqText = "hseqText";
        public const string LocalContentTitle = "localContentTitle";
        public const string LocalContentText = "localContentText";
        public const string CareersTitle = "careersTitle";
        public const string CareersText = "careersText";
        public const string ContactTitle = "contactTitle";
        public const string ContactText = "contactText";
        public const string ContactEmail = "contactEmail";
        public const string ContactPhone = "contactPhone";
        public const string ContactAddress = "contactAddress";
        public const string DemoNotice = "demoNotice";
        public const string LegalName = "legalName";
        public const string FooterText = "footerText";

        // Serviço
        public const string Summary = "summary";
        public const string IsPlanned = "isPlanned";
        public const string Icon = "icon";
    }
}
