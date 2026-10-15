using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Infra;

public class RepositorioRevistaEmArquivo : RepositorioBaseEmArquivo<Revista>, IRepositorioRevista
{
    public RepositorioRevistaEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Revista> CarregarRegistros()
    {
        return contexto.Revistas;
    }
}
