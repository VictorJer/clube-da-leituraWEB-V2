using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Apresentacao;
using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AdicionarCamadaInfraestrutura();
builder.Services.AdicionarCamadaServico();
builder.Services.AdicionaApresentacap();

var app = builder.Build();

// Configuração de Middlewares
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
