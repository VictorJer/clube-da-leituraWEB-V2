using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;

namespace ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependenciaServico
{
    public static void AdicionarCamadaServico(this IServiceCollection services)
    {
        services.AddScoped<ServicoCaixa>();
    }
}