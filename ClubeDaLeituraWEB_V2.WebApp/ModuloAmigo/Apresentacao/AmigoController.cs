using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Apresentacao;

public class AmigoController : Controller
{
    private readonly IRepositorioAmigo repositorioAmigo;
    private readonly IRepositorioEmprestimo repositorioEmprestimo;

    public AmigoController(IRepositorioAmigo repositorioAmigo, IRepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarAmigosViewModel> amigos = repositorioAmigo.SelecionarTodos()
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
    [ValidateAntiForgeryToken]// Ele garante que a requisição enviado ao seu servidor partiu realmente de um formulário do seu site, e não de um site malicioso tentando se passar pelo usuário logado.
    public ActionResult Cadastrar(CadastrarAmigoViewModel viewModel)
    {
        Amigo novoAmigo = new(viewModel.Nome, viewModel.NomeResponsavel, viewModel.Telefone);
        AdicionarErrosDeDominio(novoAmigo);
        AdicionarErroDeDuplicidade(novoAmigo);

        if (!ModelState.IsValid)
            return View(viewModel);

        repositorioAmigo.Cadastrar(novoAmigo);
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);
        if (amigo == null)
            return RedirectToAction(nameof(Listar));

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
        Amigo? amigo = repositorioAmigo.SelecionarPorId(viewModel.Id);
        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        Amigo amigoAtualizado = new(viewModel.Nome, viewModel.NomeResponsavel, viewModel.Telefone);
        AdicionarErrosDeDominio(amigoAtualizado);
        AdicionarErroDeDuplicidade(amigoAtualizado, viewModel.Id);

        if (!ModelState.IsValid)
            return View(viewModel);

        repositorioAmigo.Editar(viewModel.Id, amigoAtualizado);
        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);
        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        if (PossuiEmprestimos(amigo.Id))
        {
            TempData["Aviso"] = "Este amigo não pode ser excluído porque possui empréstimos vinculados.";
            return RedirectToAction(nameof(Listar));
        }

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
        Amigo? amigo = repositorioAmigo.SelecionarPorId(viewModel.Id);
        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        if (PossuiEmprestimos(amigo.Id))
        {
            TempData["Aviso"] = "Este amigo não pode ser excluído porque possui empréstimos vinculados.";
            return RedirectToAction(nameof(Listar));
        }

        repositorioAmigo.Excluir(viewModel.Id);

        return RedirectToAction(nameof(Listar));
    }

    private bool PossuiEmprestimos(string amigoId)
    {
        return repositorioEmprestimo.Filtrar(emprestimo => emprestimo.Amigo?.Id == amigoId).Count > 0;
    }

    private void AdicionarErrosDeDominio(Amigo amigo)
    {
        foreach (string erro in amigo.Validar())
            ModelState.AddModelError(string.Empty, erro);
    }

    private void AdicionarErroDeDuplicidade(Amigo amigo, string? idIgnorado = null)
    {
        string telefoneNormalizado = Amigo.NormalizarTelefone(amigo.Telefone);

        bool duplicado = repositorioAmigo.Filtrar(outroAmigo =>
            outroAmigo.Id != idIgnorado &&
            string.Equals(outroAmigo.Nome.Trim(), amigo.Nome.Trim(), StringComparison.OrdinalIgnoreCase) &&
            Amigo.NormalizarTelefone(outroAmigo.Telefone) == telefoneNormalizado)
            .Count > 0;

        if (duplicado)
            ModelState.AddModelError(string.Empty, "Já existe um amigo cadastrado com este nome e telefone.");
    }
}
