using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Aplicacao;

public class ServicoEmprestimo
{
    private readonly IRepositorioEmprestimo repositorioEmprestimo;
    private readonly IRepositorioAmigo repositorioAmigo;
    private readonly IRepositorioRevista repositorioRevista;

    public ServicoEmprestimo(
        IRepositorioEmprestimo repositorioEmprestimo,
        IRepositorioAmigo repositorioAmigo,
        IRepositorioRevista repositorioRevista)
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    public Result<ListarEmprestimosDto> SelecionarTodos(Guid? amigoId)
    {
        Amigo? amigoSelecionado = amigoId.HasValue
            ? repositorioAmigo.SelecionarPorId(amigoId.Value)
            : null;

        if (amigoId.HasValue && amigoSelecionado == null)
            return Result.Fail("O amigo selecionado não foi encontrado.");

        List<EmprestimoListarDto> emprestimos = repositorioEmprestimo.SelecionarTodos()
            .Where(emprestimo => amigoSelecionado == null || emprestimo.Amigo?.Id == amigoSelecionado.Id)
            .OrderByDescending(emprestimo => emprestimo.DataEmprestimo)
            .Select(emprestimo => new EmprestimoListarDto(
                emprestimo.Id,
                emprestimo.Amigo?.Id,
                emprestimo.Amigo?.Nome ?? "Amigo não encontrado",
                emprestimo.Revista?.Titulo ?? "Revista não encontrada",
                emprestimo.Revista?.NumeroEdicao ?? 0,
                emprestimo.DataEmprestimo,
                emprestimo.DataDevolucaoPrevista,
                emprestimo.DataDevolucao,
                emprestimo.Multa,
                emprestimo.Status))
            .ToList();

        return Result.Ok(new ListarEmprestimosDto(amigoSelecionado?.Id, amigoSelecionado?.Nome, emprestimos));
    }

    public OpcoesEmprestimoDto SelecionarOpcoes()
    {
        HashSet<Guid> amigosComEmprestimoAberto = repositorioEmprestimo.SelecionarTodos()
            .Where(emprestimo => !emprestimo.DataDevolucao.HasValue && emprestimo.Amigo != null)
            .Select(emprestimo => emprestimo.Amigo!.Id)
            .ToHashSet();

        List<OpcaoEmprestimoDto> amigos = repositorioAmigo.SelecionarTodos()
            .Where(amigo => !amigosComEmprestimoAberto.Contains(amigo.Id))
            .OrderBy(amigo => amigo.Nome)
            .Select(amigo => new OpcaoEmprestimoDto(amigo.Id, amigo.Nome))
            .ToList();

        List<OpcaoEmprestimoDto> revistas = repositorioRevista.SelecionarTodos()
            .Where(revista => revista.Status == StatusRevista.Disponivel)
            .OrderBy(revista => revista.Titulo)
            .ThenBy(revista => revista.NumeroEdicao)
            .Select(revista => new OpcaoEmprestimoDto(
                revista.Id,
                $"{revista.Titulo} — edição {revista.NumeroEdicao}"))
            .ToList();

        return new OpcoesEmprestimoDto(amigos, revistas);
    }

    public Result Cadastrar(CadastrarEmprestimoDto dto)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(dto.AmigoId);
        if (amigo == null)
            return Falha(nameof(dto.AmigoId), "Selecione um amigo cadastrado.");

        Revista? revista = repositorioRevista.SelecionarPorId(dto.RevistaId);
        if (revista == null)
            return Falha(nameof(dto.RevistaId), "Selecione uma revista cadastrada.");

        bool amigoPossuiEmprestimoAberto = repositorioEmprestimo.Filtrar(emprestimo =>
            emprestimo.Amigo?.Id == amigo.Id && !emprestimo.DataDevolucao.HasValue).Count > 0;

        if (amigoPossuiEmprestimoAberto)
            return Result.Fail("Este amigo já possui um empréstimo em aberto.");

        if (revista.Status != StatusRevista.Disponivel)
            return Result.Fail("A revista selecionada não está disponível para empréstimo.");

        Emprestimo emprestimo = new(amigo, revista, DateTime.Today);
        List<string> errosDeDominio = emprestimo.Validar();
        if (errosDeDominio.Count > 0)
        {
            Result resultado = Result.Ok();
            foreach (string erro in errosDeDominio)
                resultado = resultado.WithError(erro);

            return resultado;
        }

        revista.Status = StatusRevista.Emprestada;
        repositorioEmprestimo.Cadastrar(emprestimo);

        return Result.Ok();
    }

    public Result<DevolucaoEmprestimoDto> RegistrarDevolucao(Guid id)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);
        if (emprestimo == null)
            return Result.Fail("O empréstimo selecionado não foi encontrado.");

        if (emprestimo.DataDevolucao.HasValue)
            return Result.Fail("Este empréstimo já foi concluído.");

        emprestimo.DataDevolucao = DateTime.Today;
        if (emprestimo.Revista != null)
            emprestimo.Revista.Status = StatusRevista.Disponivel;

        if (!repositorioEmprestimo.Editar(emprestimo.Id, emprestimo))
            return Result.Fail("Não foi possível registrar a devolução do empréstimo.");

        return Result.Ok(new DevolucaoEmprestimoDto(emprestimo.Amigo?.Id, emprestimo.Multa));
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
}
