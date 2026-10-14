var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração de Middlewares
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
