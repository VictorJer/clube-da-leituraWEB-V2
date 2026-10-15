using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Infra;

public class RepositorioAmigoEmArquivo : RepositorioBaseEmArquivo<Amigo>, IRepositorioAmigo
{
    public RepositorioAmigoEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Amigo> CarregarRegistros()
    {
        return contexto.Amigos;
    }
}
