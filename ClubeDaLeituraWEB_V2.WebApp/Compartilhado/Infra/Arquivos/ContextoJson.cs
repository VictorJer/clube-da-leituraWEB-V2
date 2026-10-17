using System.Text.Json;
using System.Text.Json.Serialization;
using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra.Arquivos;

public sealed class ContextoJson
{
    public List<Amigo> Amigos { get; set; } = new List<Amigo>();
    public List<Caixa> Caixas { get; set; } = new List<Caixa>();
    public List<Revista> Revistas { get; set; } = new List<Revista>();
    public List<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

    private readonly string caminhoArquivo;

    public ContextoJson()
    {
        string caminhoAppData = Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string caminhoDiretorio = Path.Combine(caminhoAppData, "ClubeDaLeituraWeb");

        Directory.CreateDirectory(caminhoDiretorio);

        caminhoArquivo = Path.Combine(caminhoDiretorio, "dados.json");
    }

    public void Salvar()
    {
        JsonSerializerOptions opcoesJson = new JsonSerializerOptions();
        opcoesJson.WriteIndented = true;
        opcoesJson.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opcoesJson.ReferenceHandler = ReferenceHandler.Preserve;

        string jsonString = JsonSerializer.Serialize(this, opcoesJson);

        File.WriteAllText(caminhoArquivo, jsonString);
    }

    public void Carregar()
    {
        if (!File.Exists(caminhoArquivo))
            return;

        string jsonString = File.ReadAllText(caminhoArquivo);

        JsonSerializerOptions opcoesJson = new JsonSerializerOptions();
        opcoesJson.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opcoesJson.ReferenceHandler = ReferenceHandler.Preserve;

        ContextoJson? contextoSalvo = JsonSerializer
            .Deserialize<ContextoJson>(jsonString, opcoesJson);

        if (contextoSalvo == null)
            return;

        Amigos = contextoSalvo.Amigos ?? new List<Amigo>();
        Caixas = contextoSalvo.Caixas ?? new List<Caixa>();
        Revistas = contextoSalvo.Revistas ?? new List<Revista>();
        Emprestimos = contextoSalvo.Emprestimos ?? new List<Emprestimo>();
    }
}