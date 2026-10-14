using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using Microsoft.AspNetCore.Mvc;

public class CaixaController : Controller
{
    private readonly IRepositorioCaixa repositorioCaixa;

    public CaixaController(IRepositorioCaixa repositorioCaixa)
    {
        this.repositorioCaixa = repositorioCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarCaixasViews = new List<ListarCaixasViewModel>();

        foreach (Caixa c in caixas)
        {
            ListarCaixasViewModel vm = new ListarCaixasViewModel(
                c.Id,
                c.Etiqueta,
                c.Cor,
                c.DiasDeEmprestimo
            );

            listarCaixasViews.Add(vm);
        }

        return View(listarCaixasViews);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCaixaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        Caixa novaCaixa = new Caixa(
            vm.Etiqueta,
            vm.Cor,
            vm.DiasDeEmprestimo
        );

        repositorioCaixa.Cadastrar(novaCaixa);

        return RedirectToAction(nameof(Listar));
    }
}