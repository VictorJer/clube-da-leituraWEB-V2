using System.Globalization;
using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Apresentacao;

public class EmprestimoController : Controller
{
    private readonly ServicoEmprestimo servicoEmprestimo;

    public EmprestimoController(ServicoEmprestimo servicoEmprestimo)
    {
        this.servicoEmprestimo = servicoEmprestimo;
    }

    [HttpGet]
    public ActionResult Listar(Guid? amigoId)
    {
        if (!ModelState.IsValid)
        {
            TempData["Aviso"] = "O identificador do amigo informado é inválido.";
            return RedirectToAction(nameof(Listar));
        }

        var resultado = servicoEmprestimo.SelecionarTodos(amigoId);

        if (resultado.IsFailed)
        {
            TempData["Aviso"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(Listar));
        }

        ListarEmprestimosDto dto = resultado.Value;
        List<EmprestimoListarItemViewModel> emprestimos = dto.Emprestimos
            .Select(emprestimo => new EmprestimoListarItemViewModel(
                emprestimo.Id,
                emprestimo.AmigoId,
                emprestimo.Amigo,
                emprestimo.Revista,
                emprestimo.NumeroEdicao,
                emprestimo.DataEmprestimo,
                emprestimo.DataDevolucaoPrevista,
                emprestimo.DataDevolucao,
                emprestimo.Multa,
                emprestimo.Status))
            .ToList();

        return View(new ListarEmprestimosViewModel(dto.AmigoId, dto.NomeAmigo, emprestimos));
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        PrepararOpcoes();
        return View(new CadastrarEmprestimoViewModel(Guid.Empty, Guid.Empty));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Cadastrar(CadastrarEmprestimoViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var resultado = servicoEmprestimo.Cadastrar(
                new CadastrarEmprestimoDto(viewModel.AmigoId, viewModel.RevistaId));

            if (resultado.IsSuccess)
            {
                return RedirectToAction(nameof(Listar));
            }

            ModelState.AddModelError(resultado);
        }

        PrepararOpcoes();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RegistrarDevolucao(Guid id)
    {
        var resultado = servicoEmprestimo.RegistrarDevolucao(id);
        if (resultado.IsFailed)
        {
            TempData["Aviso"] = resultado.Errors.First().Message;
            return RedirectToAction(nameof(Listar));
        }

        DevolucaoEmprestimoDto devolucao = resultado.Value;
        TempData["Sucesso"] = devolucao.Multa > 0
            ? $"Devolução registrada. Multa por atraso: {devolucao.Multa.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}."
            : "Devolução registrada sem multa.";

        return RedirectToAction(nameof(Listar), new { amigoId = devolucao.AmigoId });
    }

    private void PrepararOpcoes()
    {
        OpcoesEmprestimoDto opcoes = servicoEmprestimo.SelecionarOpcoes();
        ViewBag.Amigos = opcoes.Amigos
            .Select(amigo => new SelectListItem(amigo.Texto, amigo.Id.ToString()))
            .ToList();
        ViewBag.Revistas = opcoes.Revistas
            .Select(revista => new SelectListItem(revista.Texto, revista.Id.ToString()))
            .ToList();
    }
}
