using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;

public class ServicoCaixa
{
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioRevista repositorioRevista;

    public ServicoCaixa(IRepositorioCaixa repositorioCaixa, IRepositorioRevista repositorioRevista)
    {
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioRevista = repositorioRevista;
    }

    public Result Cadastrar(CadastrarCaixaDto caixaDto)
    {
        if (ExiteCaixaComEtiqueta(caixaDto.Etiqueta))
            return Falha("Etiqueta", "Já existe uma caixa com esta etiqueta");

        Caixa novaCaixa = new Caixa(
            caixaDto.Etiqueta,
            caixaDto.Cor,
            caixaDto.DiasDeEmprestimo
        );

        repositorioCaixa.Cadastrar(novaCaixa);    

        return Result.Ok();    
    }

    public Result Editar(EditarCaixaDto dto)
    {
        if (ExiteCaixaComEtiqueta(dto.Etiqueta, dto.Id))
            return Falha("Etiqueta", "Já existe uma caixa com esta etiqueta");
        
        Caixa caixaAtualizada = new Caixa(
            dto.Etiqueta,
            dto.Cor,
            dto.DiasDeEmprestimo
        );

        bool conseguiuEditar = repositorioCaixa.Editar(dto.Id, caixaAtualizada);

        if (!conseguiuEditar)
            return Result.Fail("Caixa não encontrado");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return Result.Fail("Caixa não encontrada");

        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        foreach (Revista revista in revistas)
        {
            if (revista.Caixa != null && string.Equals(revista.Caixa.Id, caixa.Id))
            {
                return Result.Fail("Esta caixa não pode ser excluída porque está relacionada a uma revista!");
            }
        }        

        repositorioCaixa.Excluir(id);

        return Result.Ok();
    }

    public List<ListarCaixasDto> SelecionarTodos()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        return caixas.Select(c => new ListarCaixasDto(c.Id, c.Etiqueta, c.Cor, c.DiasDeEmprestimo)).ToList();
    }

    public Result<DetalhesCaixaDto> SelecionarPorId(Guid id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return Result.Fail("Caixa não encontrada");

        return Result.Ok(new DetalhesCaixaDto(caixa.Id, caixa.Etiqueta, caixa.Cor, caixa.DiasDeEmprestimo));
    }

    private bool ExiteCaixaComEtiqueta(string etiqueta, Guid? idnull = null)
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        foreach (Caixa c in caixas)
        {
            if (c.Id != idnull && string.Equals(c.Etiqueta, etiqueta, StringComparison.OrdinalIgnoreCase))            
                return true;          
        }

        return false;
    }
    private static Result Falha(string campo, string mensagem)
    {
        IError erro = new Error(mensagem).WithMetadata("Campo", campo);

        return Result.Fail(erro);
    }
}