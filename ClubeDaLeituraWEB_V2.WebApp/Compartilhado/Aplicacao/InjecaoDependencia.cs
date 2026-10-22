using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;

namespace ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependenciaServico
{
    public static void AdicionarCamadaServico(this IServiceCollection services)
    {
        services.AddScoped<ServicoCaixa>();
        services.AddScoped<ServicoRevista>();
    }
}