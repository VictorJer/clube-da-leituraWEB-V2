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
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        foreach (Caixa c in caixas)
        {
            if (string.Equals(c.Etiqueta, caixaDto.Etiqueta, StringComparison.OrdinalIgnoreCase))
            {
                Error errorValidacao = new Error("Já existe uma caixa com essa etiqueta")
                    .WithMetadata("Campo", nameof(c.Etiqueta));

                return Result.Fail(errorValidacao);
            }
        }

        Caixa novaCaixa = new Caixa(
            caixaDto.Etiqueta,
            caixaDto.Cor,
            caixaDto.DiasDeEmprestimo
        );

        repositorioCaixa.Cadastrar(novaCaixa);    

        return Result.Ok();    
    }

    public void Editar()
    {
        
    }

    public void Excluir()
    {
        
    }

    public void SelecionarPorId()
    {
        
    }
}