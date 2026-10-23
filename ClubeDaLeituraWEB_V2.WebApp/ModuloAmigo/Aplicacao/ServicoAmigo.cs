using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using FluentResults;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Aplicacao;

public class ServicoAmigo
{
    private readonly IRepositorioAmigo repositorioAmigo;
    private readonly IRepositorioEmprestimo repositorioEmprestimo;

    public ServicoAmigo(
        IRepositorioAmigo repositorioAmigo,
        IRepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public List<ListarAmigosDto> SelecionarTodos()
    {
        return repositorioAmigo.SelecionarTodos()
            .Select(amigo => new ListarAmigosDto(
                amigo.Id,
                amigo.Nome,
                amigo.NomeResponsavel,
                amigo.Telefone))
            .ToList();
    }

    public Result<DetalhesAmigoDto> SelecionarPorId(Guid id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);
        if (amigo == null)
            return Result.Fail("Amigo não encontrado.");

        return Result.Ok(new DetalhesAmigoDto(
            amigo.Id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone));
    }

    public Result Cadastrar(CadastrarAmigoDto dto)
    {
        Amigo amigo = new(dto.Nome, dto.NomeResponsavel, dto.Telefone);
        Result validacao = Validar(amigo);
        if (validacao.IsFailed)
            return validacao;

        if (ExisteAmigoComMesmoNomeETelefone(amigo))
            return Result.Fail("Já existe um amigo cadastrado com este nome e telefone.");

        repositorioAmigo.Cadastrar(amigo);
        return Result.Ok();
    }

    public Result Editar(EditarAmigoDto dto)
    {
        if (repositorioAmigo.SelecionarPorId(dto.Id) == null)
            return Result.Fail("Amigo não encontrado.");

        Amigo amigoAtualizado = new(dto.Nome, dto.NomeResponsavel, dto.Telefone);
        Result validacao = Validar(amigoAtualizado);
        if (validacao.IsFailed)
            return validacao;

        if (ExisteAmigoComMesmoNomeETelefone(amigoAtualizado, dto.Id))
            return Result.Fail("Já existe um amigo cadastrado com este nome e telefone.");

        if (!repositorioAmigo.Editar(dto.Id, amigoAtualizado))
            return Result.Fail("Não foi possível editar o amigo.");

        return Result.Ok();
    }

    public Result<DetalhesAmigoDto> SelecionarParaExclusao(Guid id)
    {
        Result<Amigo> resultado = BuscarAmigoExcluivel(id);
        if (resultado.IsFailed)
            return Result.Fail(resultado.Errors);

        Amigo amigo = resultado.Value;
        return Result.Ok(new DetalhesAmigoDto(
            amigo.Id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone));
    }

    public Result Excluir(Guid id)
    {
        Result<Amigo> resultado = BuscarAmigoExcluivel(id);
        if (resultado.IsFailed)
            return Result.Fail(resultado.Errors);

        if (!repositorioAmigo.Excluir(id))
            return Result.Fail("Não foi possível excluir o amigo.");

        return Result.Ok();
    }

    private Result<Amigo> BuscarAmigoExcluivel(Guid id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);
        if (amigo == null)
            return Result.Fail("Amigo não encontrado.");

        bool possuiEmprestimos = repositorioEmprestimo.Filtrar(
            emprestimo => emprestimo.Amigo?.Id == amigo.Id).Count > 0;

        if (possuiEmprestimos)
            return Result.Fail("Este amigo não pode ser excluído porque possui empréstimos vinculados.");

        return Result.Ok(amigo);
    }

    private bool ExisteAmigoComMesmoNomeETelefone(Amigo amigo, Guid? idIgnorado = null)
    {
        string telefoneNormalizado = Amigo.NormalizarTelefone(amigo.Telefone);

        return repositorioAmigo.Filtrar(outroAmigo =>
            outroAmigo.Id != idIgnorado &&
            string.Equals(outroAmigo.Nome.Trim(), amigo.Nome.Trim(), StringComparison.OrdinalIgnoreCase) &&
            Amigo.NormalizarTelefone(outroAmigo.Telefone) == telefoneNormalizado)
            .Count > 0;
    }

    private static Result Validar(Amigo amigo)
    {
        Result resultado = Result.Ok();
        foreach (string erro in amigo.Validar())
            resultado = resultado.WithError(erro);

        return resultado;
    }
}