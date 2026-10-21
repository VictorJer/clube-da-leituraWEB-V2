using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;

namespace ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao;

public static class InjecaoDependencia
{
    public static void AdicionaApresentacap(this IServiceCollection services)
    {
        services.AddControllersWithViews().AddRazorOptions(options =>
        {
            options.ViewLocationFormats.Clear();

            options.ViewLocationFormats.Add("/Modulo{1}/Apresentacao/Views/{0}.cshtml");

            options.ViewLocationFormats.Add("/Compartilhado/Apresentacao/Views/{0}.cshtml");
        });

        services.AddAutoMapper(config =>
        {
            config.AddProfile<CaixaProfile>();
        });
    }
}