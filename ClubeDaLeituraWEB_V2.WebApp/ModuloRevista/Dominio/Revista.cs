using System.ComponentModel.DataAnnotations;
using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

public enum StatusRevista
{
    [Display(Name = "Disponível")]
    Disponivel,

    [Display(Name = "Emprestada")]
    Emprestada,

    [Display(Name = "Reservada")]
    Reservada
}

public class Revista : EntidadeBase<Revista>
{
    public string Titulo { get; set; } = string.Empty;
    public int NumeroEdicao { get; set; }
    public DateTime AnoPublicacao { get; set; }
    public Caixa? Caixa { get; set; }
    public StatusRevista Status { get; set; } = StatusRevista.Disponivel;

    public Revista() { }

    public Revista(string titulo, int numeroEdicao, DateTime anoPublicacao, Caixa caixa,
        StatusRevista status = StatusRevista.Disponivel)
    {
        Titulo = (titulo ?? string.Empty).Trim();
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao.Date;
        Caixa = caixa;
        Status = status;
    }

    public override void AtualizarDados(Revista entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        NumeroEdicao = entidadeAtualizada.NumeroEdicao;
        AnoPublicacao = entidadeAtualizada.AnoPublicacao;
        Caixa = entidadeAtualizada.Caixa;
        Status = entidadeAtualizada.Status;
    }

    public override List<string> Validar()
    {
        List<string> erros = new();

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O campo \"Título\" deve ser preenchido.");
        else if (Titulo.Trim().Length < 2 || Titulo.Trim().Length > 100)
            erros.Add("O campo \"Título\" deve conter entre 2 e 100 caracteres.");

        if (NumeroEdicao <= 0)
            erros.Add("O número da edição deve ser maior que zero.");

        if (AnoPublicacao == default || AnoPublicacao.Date > DateTime.Today)
            erros.Add("Informe uma data de publicação válida, que não seja futura.");

        if (Caixa == null)
            erros.Add("Selecione uma caixa para a revista.");

        if (!Enum.IsDefined(Status))
            erros.Add("Selecione um status válido para a revista.");

        return erros;
    }
}
