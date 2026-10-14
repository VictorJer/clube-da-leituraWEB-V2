using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloHome.Apresentacao;

public class HomeController : Controller
{
    [HttpGet]
    public ActionResult Home()
    {
        return View("~/ModuloHome/Apresentacao/Views/Home.cshtml");
    }
}