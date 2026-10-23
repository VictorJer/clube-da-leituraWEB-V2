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
        JsonSerializerOptions opcoesJson = CriarOpcoesJson();
        opcoesJson.WriteIndented = true;

        string jsonString = JsonSerializer.Serialize(this, opcoesJson);

        File.WriteAllText(caminhoArquivo, jsonString);
    }

    public void Carregar()
    {
        if (!File.Exists(caminhoArquivo))
            return;

        string jsonString = File.ReadAllText(caminhoArquivo);

        JsonSerializerOptions opcoesJson = CriarOpcoesJson();

        ContextoJson? contextoSalvo = JsonSerializer
            .Deserialize<ContextoJson>(jsonString, opcoesJson);

        if (contextoSalvo == null)
            return;

        Amigos = contextoSalvo.Amigos ?? new List<Amigo>();
        Caixas = contextoSalvo.Caixas ?? new List<Caixa>();
        Revistas = contextoSalvo.Revistas ?? new List<Revista>();
        Emprestimos = contextoSalvo.Emprestimos ?? new List<Emprestimo>();
    }

    private static JsonSerializerOptions CriarOpcoesJson()
    {
        JsonSerializerOptions opcoesJson = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.Preserve
        };
        opcoesJson.Converters.Add(new IdGuidJsonConverter());

        return opcoesJson;
    }

    private sealed class IdGuidJsonConverter : JsonConverter<Guid>
    {
        public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? valor = reader.GetString();
            if (Guid.TryParse(valor, out Guid id))
                return id;

            if (valor is { Length: 7 } && valor.All(Uri.IsHexDigit))
                return Guid.ParseExact(valor.PadLeft(32, '0'), "N");

            throw new JsonException("O identificador salvo não possui um formato de GUID válido.");
        }

        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}