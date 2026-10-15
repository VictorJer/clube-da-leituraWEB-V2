using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;

public class Amigo : EntidadeBase<Amigo>
{
    public string Nome { get; set; } = string.Empty;
    public string NomeResponsavel { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    public Amigo() { }

    public Amigo(string nome, string nomeResponsavel, string telefone)
    {
        Nome = (nome ?? string.Empty).Trim();
        NomeResponsavel = (nomeResponsavel ?? string.Empty).Trim();
        Telefone = (telefone ?? string.Empty).Trim();
    }

    public static string NormalizarTelefone(string? telefone)
    {
        return new string((telefone ?? string.Empty).Where(char.IsDigit).ToArray());
        //         .Where(char.IsDigit)
        //          Filtro de números: Examina caractere por caractere e mantém apenas os dígitos numéricos (de '0' a '9').
    }

    public override List<string> Validar()
    {
        List<string> erros = new();

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O nome do amigo deve ser preenchido.");
        else if (Nome.Trim().Length < 3 || Nome.Trim().Length > 100)
            erros.Add("O nome do amigo deve conter entre 3 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(NomeResponsavel))
            erros.Add("O nome do responsável deve ser preenchido.");
        else if (NomeResponsavel.Trim().Length < 3 || NomeResponsavel.Trim().Length > 100)
            erros.Add("O nome do responsável deve conter entre 3 e 100 caracteres.");

        string digitosTelefone = NormalizarTelefone(Telefone);
        bool telefoneTemFormatoValido = Telefone.All(caractere =>
            char.IsDigit(caractere) || char.IsWhiteSpace(caractere) ||
            caractere is '+' or '(' or ')' or '-' or '.');

        if (string.IsNullOrWhiteSpace(Telefone))
            erros.Add("O telefone deve ser preenchido.");
        else if (!telefoneTemFormatoValido || digitosTelefone.Length is < 10 or > 11)
            erros.Add("O telefone deve conter 10 ou 11 dígitos.");

        return erros;
    }

    public override void AtualizarDados(Amigo entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        NomeResponsavel = entidadeAtualizada.NomeResponsavel;
        Telefone = entidadeAtualizada.Telefone;
    }
}
