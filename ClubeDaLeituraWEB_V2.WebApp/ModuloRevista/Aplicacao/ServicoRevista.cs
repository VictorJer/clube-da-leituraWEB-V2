using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Apresentacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;

public class ServicoRevista
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioEmprestimo repositorioEmprestimo;


    public ServicoRevista(IRepositorioRevista repositorioRevista, IRepositorioCaixa repositorioCaixa, IRepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public Result Cadastrar(CadastrarRevistaDto dto)
    {
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(dto.CaixaId);

        if (caixaSelecionada == null)
        {
            return Result.Fail(
                new Error("Selecione uma caixa cadastrada.")
                    .WithMetadata("Campo", nameof(dto.CaixaId))
            );            
        }

        if (ExisteRevistaComMesmoTituloEEdicao(dto.Titulo, dto.NumeroEdicao))
        {
            return Result.Fail(
                new Error("Já existe uma revista com esse título e edição.")
                    .WithMetadata("Campo", nameof(dto.NumeroEdicao))
            );
        }

        if (dto.AnoPublicacao > DateTime.Now)
        {
            return Result.Fail(
                new Error("O ano de publicação não pode ser futuro.")
                    .WithMetadata("Campo", nameof(dto.AnoPublicacao))
            );
        }

        Revista revista = new Revista(
            dto.Titulo,
            dto.NumeroEdicao,
            dto.AnoPublicacao,
            caixaSelecionada,
            dto.Status
        );

        repositorioRevista.Cadastrar(revista);

        return Result.Ok();
    }

    public Result Editar(EditarRevistaDto dto)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(dto.Id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");
        
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(dto.CaixaId);

        if (caixaSelecionada == null)
        {
            return Result.Fail(
                new Error("Selecione uma caixa cadastrada.")
                    .WithMetadata("Campo", nameof(dto.CaixaId))
            );            
        }

        bool emprestimoAtivo = repositorioEmprestimo.Filtrar(emprestimo =>
            emprestimo.Revista?.Id == revista.Id && !emprestimo.DataDevolucao.HasValue).Count > 0;

        if (emprestimoAtivo && dto.Status != StatusRevista.Emprestada)
        {
            return Result.Fail(
                new Error("A revista não pode deixar o status emprestada enquanto o empréstimo estiver aberto.")
                    .WithMetadata("Campo", nameof(dto.Status))
            );
        }

        if (ExisteRevistaComMesmoTituloEEdicao(dto.Titulo, dto.NumeroEdicao, dto.Id))
        {
            return Result.Fail(
                new Error("Já existe uma revista com esse título e edição.")
                    .WithMetadata("Campo", nameof(dto.NumeroEdicao))
            );
        }

        if (dto.AnoPublicacao > DateTime.Now)
        {
            return Result.Fail(
                new Error("O ano de publicação não pode ser futuro.")
                    .WithMetadata("Campo", nameof(dto.AnoPublicacao))
            );
        }

        repositorioRevista.Editar(dto.Id, new Revista(dto.Titulo, dto.NumeroEdicao, dto.AnoPublicacao, caixaSelecionada, dto.Status));

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");

        bool possuiEmprestimos = repositorioEmprestimo.Filtrar(emprestimo =>
            emprestimo.Revista?.Id == revista.Id).Count > 0;

        if (possuiEmprestimos)
            return Result.Fail("Esta revista não pode ser excluída porque possui empréstimos vinculados.");

        repositorioRevista.Excluir(id);

        return Result.Ok();
    }

    private bool ExisteRevistaComMesmoTituloEEdicao(string titulo, int numeroEdicao, Guid? idIgnorado = null)
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        return revistas.Any(r => 
            r.Id != idIgnorado &&
            string.Equals(r.Titulo, titulo, StringComparison.OrdinalIgnoreCase) &&
            r.NumeroEdicao == numeroEdicao
            );
    }

    public Result<DetalhesRevistaDto> SelecionarPorId(Guid id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");               
        
        return new DetalhesRevistaDto(
            id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa.Id,
            revista.Caixa.Etiqueta,
            revista.Status
        );
    }

    public List<ListarRevistasDto> SelecionarTodos()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        return revistas.Select(r => new ListarRevistasDto(r.Id, r.Titulo, r.NumeroEdicao, r.AnoPublicacao, r.Caixa.Etiqueta, r.Status)).ToList();
    }
}
