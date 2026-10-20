using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Infra;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Infra;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Infra;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Infra;

namespace ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AdicionarCamadaInfraestrutura(this IServiceCollection services)
    {
        services.AddScoped(serviceProvider =>
        {
            ContextoJson contexto = new ContextoJson();
            contexto.Carregar();
            return contexto;
        });


        services.AddScoped<IRepositorioCaixa, RepositorioCaixaEmArquivo>();
        services.AddScoped<IRepositorioAmigo, RepositorioAmigoEmArquivo>();
        services.AddScoped<IRepositorioRevista, RepositorioRevistaEmArquivo>();
        services.AddScoped<IRepositorioEmprestimo, RepositorioEmprestimoEmArquivo>();
    }
}