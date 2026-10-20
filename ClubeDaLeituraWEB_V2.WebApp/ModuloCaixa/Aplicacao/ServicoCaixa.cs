using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;

public class ServicoCaixa
{
    private readonly IRepositorioCaixa repositorioCaixa;

    public ServicoCaixa(IRepositorioCaixa repositorioCaixa)
    {
        this.repositorioCaixa = repositorioCaixa;
    }

    public void Cadastrar(CadastrarCaixaDto caixaDto)
    {
        Caixa novaCaixa = new Caixa(
            caixaDto.Etiqueta,
            caixaDto.Cor,
            caixaDto.DiasDeEmprestimo
        );

        repositorioCaixa.Cadastrar(novaCaixa);        
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