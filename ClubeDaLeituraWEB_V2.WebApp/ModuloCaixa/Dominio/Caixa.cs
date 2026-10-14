using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;

public sealed class Caixa : EntidadeBase<Caixa>
{
    public string Etiqueta { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public int DiasDeEmprestimo { get; set; } = 7;

    public Caixa() { }

    public Caixa(string etiqueta, string cor, int diasDeEmprestimo)
    {
        Etiqueta = etiqueta;
        Cor = cor;
        DiasDeEmprestimo = diasDeEmprestimo;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Etiqueta))
            erros.Add("O campo \"Etiqueta\" deve ser preenchido.");

        else if (Etiqueta.Length > 50)
            erros.Add("O campo \"Etiqueta\" deve conter no máximo 50 caracteres.");

        if (DiasDeEmprestimo < 1)
            erros.Add("O campo \"Dias de Empréstimo\" deve conter um valor maior que 0.");

        return erros;
    }

    public override void AtualizarDados(Caixa entidadeAtualizada)
    {
        Etiqueta = entidadeAtualizada.Etiqueta;
        Cor = entidadeAtualizada.Cor;
        DiasDeEmprestimo = entidadeAtualizada.DiasDeEmprestimo;
    }
}