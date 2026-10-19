using System.Globalization;
using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Apresentacao;

public class EmprestimoController : Controller
{
    private readonly IRepositorioEmprestimo repositorioEmprestimo;
    private readonly IRepositorioAmigo repositorioAmigo;
    private readonly IRepositorioRevista repositorioRevista;

    public EmprestimoController(
        IRepositorioEmprestimo repositorioEmprestimo,
        IRepositorioAmigo repositorioAmigo,
        IRepositorioRevista repositorioRevista)
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    [HttpGet]
    public ActionResult Listar(string? amigoId)
    {
        Amigo? amigoSelecionado = string.IsNullOrWhiteSpace(amigoId)
            ? null
            : repositorioAmigo.SelecionarPorId(amigoId);

        if (!string.IsNullOrWhiteSpace(amigoId) && amigoSelecionado == null)
        {
            TempData["Aviso"] = "O amigo selecionado não foi encontrado.";
            return RedirectToAction(nameof(Listar));
        }

        List<EmprestimoListarItemViewModel> emprestimos = repositorioEmprestimo.SelecionarTodos()
            .Where(emprestimo => amigoSelecionado == null || emprestimo.Amigo?.Id == amigoSelecionado.Id)
            .OrderByDescending(emprestimo => emprestimo.DataEmprestimo)
            .Select(emprestimo => new EmprestimoListarItemViewModel(
                emprestimo.Id,
                emprestimo.Amigo?.Id ?? string.Empty,
                emprestimo.Amigo?.Nome ?? "Amigo não encontrado",
                emprestimo.Revista?.Titulo ?? "Revista não encontrada",
                emprestimo.Revista?.NumeroEdicao ?? 0,
                emprestimo.DataEmprestimo,
                emprestimo.DataDevolucaoPrevista,
                emprestimo.DataDevolucao,
                emprestimo.Multa,
                emprestimo.Status))
            .ToList();

        return View(new ListarEmprestimosViewModel(amigoSelecionado?.Id, amigoSelecionado?.Nome, emprestimos));
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        PrepararOpcoes();
        return View(new CadastrarEmprestimoViewModel(string.Empty, string.Empty));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Cadastrar(CadastrarEmprestimoViewModel viewModel)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(viewModel.AmigoId);
        Revista? revista = repositorioRevista.SelecionarPorId(viewModel.RevistaId);

        if (amigo == null)
            ModelState.AddModelError(nameof(viewModel.AmigoId), "Selecione um amigo cadastrado.");

        if (revista == null)
            ModelState.AddModelError(nameof(viewModel.RevistaId), "Selecione uma revista cadastrada.");

        if (amigo != null && repositorioEmprestimo.Filtrar(emprestimo =>
                emprestimo.Amigo?.Id == amigo.Id && !emprestimo.DataDevolucao.HasValue).Count > 0)
            ModelState.AddModelError(string.Empty, "Este amigo já possui um empréstimo em aberto.");

        if (revista != null && revista.Status != StatusRevista.Disponivel)
            ModelState.AddModelError(string.Empty, "A revista selecionada não está disponível para empréstimo.");

        if (amigo != null && revista != null)
        {
            Emprestimo emprestimo = new(amigo, revista, DateTime.Today);
            AdicionarErrosDeDominio(emprestimo);

            if (ModelState.IsValid)
            {
                revista.Status = StatusRevista.Emprestada;
                repositorioEmprestimo.Cadastrar(emprestimo);
                return RedirectToAction(nameof(Listar));
            }
        }

        PrepararOpcoes();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RegistrarDevolucao(string id)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);
        if (emprestimo == null)
        {
            TempData["Aviso"] = "O empréstimo selecionado não foi encontrado.";
            return RedirectToAction(nameof(Listar));
        }

        if (emprestimo.DataDevolucao.HasValue)
        {
            TempData["Aviso"] = "Este empréstimo já foi concluído.";
            return RedirectToAction(nameof(Listar), new { amigoId = emprestimo.Amigo?.Id });
        }

        emprestimo.DataDevolucao = DateTime.Today;
        if (emprestimo.Revista != null)
            emprestimo.Revista.Status = StatusRevista.Disponivel;

        repositorioEmprestimo.Editar(emprestimo.Id, emprestimo);
        TempData["Sucesso"] = emprestimo.Multa > 0
            ? $"Devolução registrada. Multa por atraso: {emprestimo.Multa.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}."
            : "Devolução registrada sem multa.";
        return RedirectToAction(nameof(Listar), new { amigoId = emprestimo.Amigo?.Id });
    }

    private void AdicionarErrosDeDominio(Emprestimo emprestimo)
    {
        foreach (string erro in emprestimo.Validar())
            ModelState.AddModelError(string.Empty, erro);
    }

    private void PrepararOpcoes()
    {
        HashSet<string> amigosComEmprestimoAberto = repositorioEmprestimo.SelecionarTodos()
            .Where(emprestimo => !emprestimo.DataDevolucao.HasValue && emprestimo.Amigo != null)
            .Select(emprestimo => emprestimo.Amigo!.Id)
            .ToHashSet();

        ViewBag.Amigos = repositorioAmigo.SelecionarTodos()
            .Where(amigo => !amigosComEmprestimoAberto.Contains(amigo.Id))
            .OrderBy(amigo => amigo.Nome)
            .Select(amigo => new SelectListItem(amigo.Nome, amigo.Id))
            .ToList();

        ViewBag.Revistas = repositorioRevista.SelecionarTodos()
            .Where(revista => revista.Status == StatusRevista.Disponivel)
            .OrderBy(revista => revista.Titulo)
            .ThenBy(revista => revista.NumeroEdicao)
            .Select(revista => new SelectListItem($"{revista.Titulo} — edição {revista.NumeroEdicao}", revista.Id))
            .ToList();
    }
}
