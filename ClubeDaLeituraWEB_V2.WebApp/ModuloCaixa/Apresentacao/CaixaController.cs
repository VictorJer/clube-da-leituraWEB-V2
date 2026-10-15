using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;

public class CaixaController : Controller
{
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioRevista repositorioRevista;

    public CaixaController(IRepositorioCaixa repositorioCaixa, IRepositorioRevista repositorioRevista)
    {
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioRevista = repositorioRevista;
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
        CadastrarCaixaViewModel viewModel = new CadastrarCaixaViewModel(
            string.Empty,
            "#763c45",
            7
        );

        return View(viewModel);
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

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(id);

        if (caixaSelecionada == null)
            return RedirectToAction(nameof(Listar));

        EditarCaixaViewModel viewModel = new EditarCaixaViewModel(
            caixaSelecionada.Id,
            caixaSelecionada.Etiqueta,
            caixaSelecionada.Cor,
            caixaSelecionada.DiasDeEmprestimo
        );

        return View(viewModel);
    }

    [HttpPost]
    public ActionResult Editar(EditarCaixaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        Caixa caixaAtualizada = new Caixa(
            vm.Etiqueta,
            vm.Cor,
            vm.DiasDeEmprestimo
        );

        repositorioCaixa.Editar(vm.Id, caixaAtualizada);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        if (repositorioRevista.Filtrar(revista => revista.Caixa?.Id == id).Count > 0)
        {
            TempData["Aviso"] = "Esta caixa não pode ser excluída porque possui revistas vinculadas.";
            return RedirectToAction(nameof(Listar));
        }

        ExcluirCaixaViewModel excluirVm = new ExcluirCaixaViewModel(
            id,
            caixa.Etiqueta,
            caixa.Cor,
            caixa.DiasDeEmprestimo
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(excluirVm.Id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        if (repositorioRevista.Filtrar(revista => revista.Caixa?.Id == excluirVm.Id).Count > 0)
        {
            TempData["Aviso"] = "Esta caixa não pode ser excluída porque possui revistas vinculadas.";
            return RedirectToAction(nameof(Listar));
        }

        repositorioCaixa.Excluir(caixa);

        return RedirectToAction(nameof(Listar));
    }
}