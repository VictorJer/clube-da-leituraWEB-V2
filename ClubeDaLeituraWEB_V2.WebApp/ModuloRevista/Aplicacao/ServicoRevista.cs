using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;

public class ServicoRevista
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;


    public ServicoRevista(IRepositorioRevista repositorioRevista, IRepositorioCaixa repositorioCaixa)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    public Result Cadastrar(CadastrarRevistaDto dto)
    {
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(dto.CaixaId);

        if (ExisteRevistaComMesmoTituloEEdicao(dto.Titulo, dto.NumeroEdicao))
        {
            return Result.Fail(
                new Error("Já existe uma revista com esse título e edição.")
                    .WithMetadata("Campo", "NumeroEdicao")
            );
        }

        Revista revista = new Revista(
            dto.Titulo,
            dto.NumeroEdicao,
            dto.AnoPublicacao,
            caixaSelecionada,
            dto.Status
        );

        return Result.Ok();
    }

    private bool ExisteRevistaComMesmoTituloEEdicao(string titulo, int numeroEdicao, string? idIgnorado = null)
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        return revistas.Any(r => 
            r.Id != idIgnorado &&
            string.Equals(r.Titulo, titulo, StringComparison.OrdinalIgnoreCase) &&
            r.NumeroEdicao == numeroEdicao
            );
    }

    public List<ListarRevistasDto> SelecionarTodos()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        return revistas.Select(r => new ListarRevistasDto(r.Id, r.Titulo, r.NumeroEdicao, r.AnoPublicacao, r.Caixa.Etiqueta, r.Status)).ToList();
    }
}