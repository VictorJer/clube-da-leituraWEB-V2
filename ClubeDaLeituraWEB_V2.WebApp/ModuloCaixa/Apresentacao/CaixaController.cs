using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;
using FluentResults;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class CaixaController : Controller
{
    private readonly ServicoCaixa servicoCaixa;
    private readonly IMapper mapper;

    public CaixaController(ServicoCaixa servicoCaixa, IMapper mapper)
    {
        this.servicoCaixa = servicoCaixa;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCaixasDto> caixasDtos = servicoCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarCaixasViews = mapper.Map<List<ListarCaixasViewModel>>(caixasDtos);

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
    [ValidateAntiForgeryToken]
    public ActionResult Cadastrar(CadastrarCaixaViewModel vm)
    {

        if (!ModelState.IsValid)
            return View(vm);        

        CadastrarCaixaDto dto = mapper.Map<CadastrarCaixaDto>(vm);

        var result = servicoCaixa.Cadastrar(dto);

        if (result.IsFailed)
        {
            ModelState.AddModelError(result);

            return View(vm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Result<DetalhesCaixaDto> result = servicoCaixa.SelecionarPorId(id);

        if (result.IsFailed)
        {
            TempData.AddErrorMessagem(result);

            return RedirectToAction(nameof(Listar));
        }

        DetalhesCaixaDto dto = result.Value;

        EditarCaixaViewModel viewModel = mapper.Map<EditarCaixaViewModel>(dto);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(EditarCaixaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        EditarCaixaDto dto = mapper.Map<EditarCaixaDto>(vm);

        var result = servicoCaixa.Editar(dto);

        if (result.IsFailed)
        {
            ModelState.AddModelError(result);

            return View(vm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Result<DetalhesCaixaDto> result = servicoCaixa.SelecionarPorId(id);

        if (result.IsFailed)
        {
            TempData.AddErrorMessagem(result);

            return RedirectToAction(nameof(Listar));            
        }

        DetalhesCaixaDto dto = result.Value;

        ExcluirCaixaViewModel excluirVm = mapper.Map<ExcluirCaixaViewModel>(dto);

        return View(excluirVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        Result result = servicoCaixa.Excluir(excluirVm.Id);

        if (result.IsFailed)
        {
            TempData.AddErrorMessagem(result);
            
            return RedirectToAction(nameof(Listar));
        }

        return RedirectToAction(nameof(Listar));
    }
}