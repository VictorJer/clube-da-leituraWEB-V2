using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Apresentacao;

public class AmigoController : Controller
{
    private readonly ServicoAmigo servicoAmigo;

    public AmigoController(ServicoAmigo servicoAmigo)
    {
        this.servicoAmigo = servicoAmigo;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarAmigosViewModel> amigos = servicoAmigo.SelecionarTodos()
            .Select(amigo => new ListarAmigosViewModel(
                amigo.Id,
                amigo.Nome,
                amigo.NomeResponsavel,
                amigo.Telefone))
            .ToList();

        return View(amigos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View(new CadastrarAmigoViewModel(string.Empty, string.Empty, string.Empty));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Cadastrar(CadastrarAmigoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Result resultado = servicoAmigo.Cadastrar(new CadastrarAmigoDto(
            viewModel.Nome,
            viewModel.NomeResponsavel,
            viewModel.Telefone));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesAmigoDto> resultado = servicoAmigo.SelecionarPorId(id);
        if (resultado.IsFailed)
        {
            TempData["Aviso"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(Listar));
        }

        DetalhesAmigoDto amigo = resultado.Value;
        EditarAmigoViewModel viewModel = new(
            amigo.Id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(EditarAmigoViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        Result resultado = servicoAmigo.Editar(new EditarAmigoDto(
            viewModel.Id,
            viewModel.Nome,
            viewModel.NomeResponsavel,
            viewModel.Telefone));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(viewModel);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<DetalhesAmigoDto> resultado = servicoAmigo.SelecionarParaExclusao(id);
        if (resultado.IsFailed)
        {
            TempData["Aviso"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(Listar));
        }

        DetalhesAmigoDto amigo = resultado.Value;
        ExcluirAmigoViewModel viewModel = new(
            amigo.Id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Excluir(ExcluirAmigoViewModel viewModel)
    {
        Result resultado = servicoAmigo.Excluir(viewModel.Id);
        if (resultado.IsFailed)
        {
            TempData["Aviso"] = resultado.Errors.First().Message;
        }

        return RedirectToAction(nameof(Listar));
    }
}
