using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using FluentResults;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;

public class ServicoCaixa
{
    private readonly IRepositorioCaixa repositorioCaixa;

    public ServicoCaixa(IRepositorioCaixa repositorioCaixa)
    {
        this.repositorioCaixa = repositorioCaixa;
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

    public void Excluir()
    {
        
    }

    public void SelecionarPorId()
    {
        
    }

    private bool ExiteCaixaComEtiqueta(string etiqueta, string? idnull = null)
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