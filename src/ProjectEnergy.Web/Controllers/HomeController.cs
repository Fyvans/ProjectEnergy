using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectEnergy.Web.Models;

namespace ProjectEnergy.Web.Controllers;

/// <summary>
/// Páginas institucionais. O conteúdo atual é de demonstração e vive nas views por idioma.
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult About() => View();

    public IActionResult Services() => View();

    public IActionResult Hseq() => View();

    public IActionResult LocalContent() => View();

    public IActionResult Careers() => View();

    public IActionResult Contact() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
