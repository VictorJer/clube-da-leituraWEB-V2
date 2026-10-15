using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Infra;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Infra;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped(serviceProvider =>
{
    ContextoJson contexto = new ContextoJson();
    contexto.Carregar();
    return contexto;
});


builder.Services.AddScoped<IRepositorioCaixa, RepositorioCaixaEmArquivo>();
builder.Services.AddScoped<IRepositorioRevista, RepositorioRevistaEmArquivo>();


builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    options.ViewLocationFormats.Clear();

    options.ViewLocationFormats.Add("/Modulo{1}/Apresentacao/Views/{0}.cshtml");

    options.ViewLocationFormats.Add("/Compartilhado/Apresentacao/Views/{0}.cshtml");
});


var app = builder.Build();

// Configuração de Middlewares
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
