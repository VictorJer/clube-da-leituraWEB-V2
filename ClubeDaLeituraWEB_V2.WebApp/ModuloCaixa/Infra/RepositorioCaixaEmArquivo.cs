using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Infra;

public class RepositorioCaixaEmArquivo : RepositorioBaseEmArquivo<Caixa>, IRepositorioCaixa
{
    public RepositorioCaixaEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Caixa> CarregarRegistros()
    {
        return contexto.Caixas;
    }
}