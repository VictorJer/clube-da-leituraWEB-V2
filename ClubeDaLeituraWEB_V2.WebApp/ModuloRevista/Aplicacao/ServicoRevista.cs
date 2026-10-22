using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;

public class ServicoRevista
{
    private readonly IRepositorioRevista repositorioRevista;

    public ServicoRevista(IRepositorioRevista repositorioRevista)
    {
        this.repositorioRevista = repositorioRevista;
    }
    public List<ListarRevistasDto> SelecionarTodos()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        return revistas.Select(r => new ListarRevistasDto(r.Id, r.Titulo, r.NumeroEdicao, r.AnoPublicacao, r.Caixa.Etiqueta, r.Status)).ToList();
    }
}