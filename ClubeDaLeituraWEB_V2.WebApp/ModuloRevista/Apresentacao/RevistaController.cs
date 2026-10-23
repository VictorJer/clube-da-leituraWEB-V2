using AutoMapper;
using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Apresentacao;

public class RevistaController : Controller
{
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IMapper mapper;
    private readonly ServicoRevista servicoRevista;

    public RevistaController(
        IRepositorioCaixa repositorioCaixa,
        IMapper mapper,
        ServicoRevista servicoRevista)
    {
        this.repositorioCaixa = repositorioCaixa;
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
        Result<DetalhesRevistaDto> result = servicoRevista.SelecionarPorId(id);

        if (result.IsFailed)
        {
            TempData.AddErrorMessagem(result);

            return RedirectToAction(nameof(Listar));
        }

        ExcluirRevistaViewModel revistaViewModel = mapper.Map<ExcluirRevistaViewModel>(result.Value);

        return View(revistaViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Excluir(ExcluirRevistaViewModel viewModel)
    {
        var result = servicoRevista.Excluir(viewModel.Id);

        if (result.IsFailed)
            TempData.AddErrorMessagem(result);

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
