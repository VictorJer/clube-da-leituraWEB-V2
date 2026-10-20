using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

public class CaixaController : Controller
{
    private readonly ServicoCaixa servicoCaixa;

    public CaixaController(ServicoCaixa servicoCaixa)
    {
        this.servicoCaixa = servicoCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCaixasDto> caixas = servicoCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarCaixasViews = new List<ListarCaixasViewModel>();

        foreach (ListarCaixasDto c in caixas)
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
    [ValidateAntiForgeryToken]
    public ActionResult Cadastrar(CadastrarCaixaViewModel vm)
    {

        if (!ModelState.IsValid)
            return View(vm);        

        CadastrarCaixaDto dto = new CadastrarCaixaDto(
            vm.Etiqueta,
            vm.Cor,
            vm.DiasDeEmprestimo
        );

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

        EditarCaixaViewModel viewModel = new EditarCaixaViewModel(
            dto.Id,
            dto.Etiqueta,
            dto.Cor,
            dto.DiasDeEmprestimo
        );

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(EditarCaixaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        EditarCaixaDto dto = new EditarCaixaDto(
            vm.Id,
            vm.Etiqueta,
            vm.Cor,
            vm.DiasDeEmprestimo
        );

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

        // if (repositorioRevista.Filtrar(revista => revista.Caixa?.Id == id).Count > 0)
        // {
        //     TempData["Aviso"] = "Esta caixa não pode ser excluída porque possui revistas vinculadas.";
        //     return RedirectToAction(nameof(Listar));
        // }

        DetalhesCaixaDto dto = result.Value;

        ExcluirCaixaViewModel excluirVm = new ExcluirCaixaViewModel(
            id,
            dto.Etiqueta,
            dto.Cor,
            dto.DiasDeEmprestimo
        );

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