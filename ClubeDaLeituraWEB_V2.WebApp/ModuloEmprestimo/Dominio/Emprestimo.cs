using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

public class Emprestimo : EntidadeBase<Emprestimo>
{
    public const decimal ValorMultaPorDia = 2m;

    public Amigo? Amigo { get; set; }
    public Revista? Revista { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucaoPrevista { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public decimal Multa => DataDevolucao.HasValue
        ? Math.Max(0, (DataDevolucao.Value.Date - DataDevolucaoPrevista.Date).Days) * ValorMultaPorDia
        : 0m;
    public StatusEmprestimo Status => DataDevolucao.HasValue
        ? StatusEmprestimo.Concluido
        : DataDevolucaoPrevista.Date < DateTime.Today
            ? StatusEmprestimo.Atrasado
            : StatusEmprestimo.Aberto;

    public Emprestimo() { }

    public Emprestimo(Amigo amigo, Revista revista, DateTime dataEmprestimo)
    {
        Amigo = amigo;
        Revista = revista;
        DataEmprestimo = dataEmprestimo.Date;
        DataDevolucaoPrevista = dataEmprestimo.Date.AddDays(revista.Caixa?.DiasDeEmprestimo ?? 0);
    }

    public override void AtualizarDados(Emprestimo entidadeAtualizada)
    {
        Amigo = entidadeAtualizada.Amigo;
        Revista = entidadeAtualizada.Revista;
        DataEmprestimo = entidadeAtualizada.DataEmprestimo;
        DataDevolucaoPrevista = entidadeAtualizada.DataDevolucaoPrevista;
        DataDevolucao = entidadeAtualizada.DataDevolucao;
    }

    public override List<string> Validar()
    {
        List<string> erros = new();

        if (Amigo == null)
            erros.Add("Selecione um amigo cadastrado.");

        if (Revista == null)
            erros.Add("Selecione uma revista cadastrada.");

        if (DataEmprestimo == default || DataEmprestimo.Date > DateTime.Today)
            erros.Add("A data do empréstimo deve ser válida e não pode ser futura.");

        if (DataDevolucaoPrevista == default || DataDevolucaoPrevista.Date <= DataEmprestimo.Date)
            erros.Add("A data prevista de devolução deve ser posterior à data do empréstimo.");

        if (DataDevolucao.HasValue && DataDevolucao.Value.Date < DataEmprestimo.Date)
            erros.Add("A data de devolução não pode ser anterior à data do empréstimo.");

        return erros;
    }
}
