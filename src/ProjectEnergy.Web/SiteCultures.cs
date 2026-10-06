namespace ProjectEnergy.Web;

/// <summary>
/// Idiomas suportados pelo website. O idioma por omissão (pt) é provisório até decisão aprovada.
/// O padrão de rota em Program.cs deve acompanhar esta lista.
/// </summary>
public static class SiteCultures
{
    public const string Default = "pt";

    public static readonly string[] Supported = ["pt", "en"];
}
