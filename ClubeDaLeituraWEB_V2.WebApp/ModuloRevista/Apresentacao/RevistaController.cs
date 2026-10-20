using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Apresentacao;

public class RevistaController : Controller
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioEmprestimo repositorioEmprestimo;

    public RevistaController(
        IRepositorioRevista repositorioRevista,
        IRepositorioCaixa repositorioCaixa,
        IRepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarRevistasViewModel> revistas = repositorioRevista.SelecionarTodos()
            .Select(revista => new ListarRevistasViewModel(
                revista.Id,
                revista.Titulo,
                revista.NumeroEdicao,
                revista.AnoPublicacao,
                revista.Caixa?.Etiqueta ?? "Caixa não encontrada",
                revista.Status))
            .ToList();

        return View(revistas);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        PrepararCaixas();
        CadastrarRevistaViewModel viewModel = new(
            string.Empty,
            1,
            DateTime.Today,
            string.Empty,
            StatusRevista.Disponivel);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Cadastrar(CadastrarRevistaViewModel viewModel)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(viewModel.CaixaId);
        if (caixa == null)
            ModelState.AddModelError(nameof(viewModel.CaixaId), "Selecione uma caixa cadastrada.");

        if (viewModel.AnoPublicacao > DateTime.Now)
            ModelState.AddModelError(nameof(viewModel.AnoPublicacao), "");

        if (caixa != null)
        {
            Revista revista = new(
                viewModel.Titulo,
                viewModel.NumeroEdicao,
                viewModel.AnoPublicacao,
                caixa,
                viewModel.Status);

            AdicionarErrosDeDominio(revista);
            if (ModelState.IsValid)
                repositorioRevista.Cadastrar(revista);
        }

        if (!ModelState.IsValid)
        {
            PrepararCaixas();
            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);
        if (revista == null)
            return RedirectToAction(nameof(Listar));

        PrepararCaixas();
        EditarRevistaViewModel viewModel = new(
            revista.Id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa?.Id ?? string.Empty,
            revista.Status);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(EditarRevistaViewModel viewModel)
    {
        Revista? revistaAtual = repositorioRevista.SelecionarPorId(viewModel.Id);
        if (revistaAtual == null)
            return RedirectToAction(nameof(Listar));

        Caixa? caixa = repositorioCaixa.SelecionarPorId(viewModel.CaixaId);
        if (caixa == null)
            ModelState.AddModelError(nameof(viewModel.CaixaId), "Selecione uma caixa cadastrada.");

        bool emprestimoAtivo = PossuiEmprestimoAtivo(revistaAtual.Id);
        if (emprestimoAtivo && viewModel.Status != StatusRevista.Emprestada)
            ModelState.AddModelError(nameof(viewModel.Status), "A revista não pode deixar o status emprestada enquanto o empréstimo estiver aberto.");

        if (!ModelState.IsValid)
        {
            PrepararCaixas();
            return View(viewModel);
        }

        if (caixa != null)
        {
            Revista revistaAtualizada = new(
                viewModel.Titulo,
                viewModel.NumeroEdicao,
                viewModel.AnoPublicacao,
                caixa,
                viewModel.Status);

            AdicionarErrosDeDominio(revistaAtualizada);
            if (ModelState.IsValid)
                repositorioRevista.Editar(viewModel.Id, revistaAtualizada);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);
        if (revista == null)
            return RedirectToAction(nameof(Listar));

        if (repositorioEmprestimo.Filtrar(emprestimo => emprestimo.Revista?.Id == revista.Id).Count > 0)
        {
            TempData["Aviso"] = "Esta revista não pode ser excluída porque possui empréstimos vinculados.";
            return RedirectToAction(nameof(Listar));
        }

        ExcluirRevistaViewModel viewModel = new(
            revista.Id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa?.Etiqueta ?? "Caixa não encontrada",
            revista.Status);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Excluir(ExcluirRevistaViewModel viewModel)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(viewModel.Id);
        if (revista == null)
            return RedirectToAction(nameof(Listar));

        if (repositorioEmprestimo.Filtrar(emprestimo => emprestimo.Revista?.Id == revista.Id).Count > 0)
        {
            TempData["Aviso"] = "Esta revista não pode ser excluída porque possui empréstimos vinculados.";
            return RedirectToAction(nameof(Listar));
        }

        repositorioRevista.Excluir(revista.Id);

        return RedirectToAction(nameof(Listar));
    }

    private void AdicionarErrosDeDominio(Revista revista)
    {
        foreach (string erro in revista.Validar())
            ModelState.AddModelError(string.Empty, erro);
    }

    private void PrepararCaixas()
    {
        ViewBag.Caixas = repositorioCaixa.SelecionarTodos()
            .OrderBy(caixa => caixa.Etiqueta)
            .Select(caixa => new SelectListItem(caixa.Etiqueta, caixa.Id))
            .ToList();
    }

    private bool PossuiEmprestimoAtivo(string revistaId)
    {
        return repositorioEmprestimo.Filtrar(emprestimo =>
            emprestimo.Revista?.Id == revistaId && !emprestimo.DataDevolucao.HasValue).Count > 0;
    }
}
