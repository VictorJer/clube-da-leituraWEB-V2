using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Infra;

public class RepositorioEmprestimoEmArquivo : RepositorioBaseEmArquivo<Emprestimo>, IRepositorioEmprestimo
{
    public RepositorioEmprestimoEmArquivo(ContextoJson contexto) : base(contexto) { }

    protected override List<Emprestimo> CarregarRegistros()
    {
        return contexto.Emprestimos;
    }
}
