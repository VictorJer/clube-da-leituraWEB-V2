using AutoMapper;
using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Apresentacao;

public class RevistaController : Controller
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioEmprestimo repositorioEmprestimo;
    private readonly IMapper mapper;
    private readonly ServicoRevista servicoRevista;

    public RevistaController(
        IRepositorioRevista repositorioRevista,
        IRepositorioCaixa repositorioCaixa,
        IRepositorioEmprestimo repositorioEmprestimo,
        IMapper mapper,
        ServicoRevista servicoRevista)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.mapper = mapper;
        this.servicoRevista = servicoRevista;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarRevistasDto> dto = servicoRevista.SelecionarTodos();

        List<ListarRevistasViewModel> vm = mapper.Map<List<ListarRevistasViewModel>>(dto);

        return View(vm);
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
        if (!ModelState.IsValid)
        {
            PrepararCaixas();
            return View(viewModel);
        }

        CadastrarRevistaDto revistaDto = mapper.Map<CadastrarRevistaDto>(viewModel);

        var result = servicoRevista.Cadastrar(revistaDto);

        if (result.IsFailed)
        {
            ModelState.AddModelError(result);

            PrepararCaixas();
            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Result<DetalhesRevistaDto> result = servicoRevista.SelecionarPorId(id);

        if (result.IsFailed)
        {
            TempData.AddErrorMessagem(result);

            return RedirectToAction(nameof(Listar));
        }

       EditarRevistaViewModel viewModel = mapper.Map<EditarRevistaViewModel>(result.Value);
        PrepararCaixas();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(EditarRevistaViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            PrepararCaixas();
            return View(viewModel);
        }

        EditarRevistaDto revistaDto = mapper.Map<EditarRevistaDto>(viewModel);
        var result = servicoRevista.Editar(revistaDto);

        if (result.IsFailed)
        {
            ModelState.AddModelError(result);
            PrepararCaixas();
            return View(viewModel);
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

    private void PrepararCaixas()
    {
        ViewBag.Caixas = repositorioCaixa.SelecionarTodos()
            .OrderBy(caixa => caixa.Etiqueta)
            .Select(caixa => new SelectListItem(caixa.Etiqueta, caixa.Id))
            .ToList();
    }


}
