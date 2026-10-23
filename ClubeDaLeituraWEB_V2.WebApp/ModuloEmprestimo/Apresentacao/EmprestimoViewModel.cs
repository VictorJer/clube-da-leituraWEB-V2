using System.ComponentModel.DataAnnotations;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Apresentacao;

public record EmprestimoListarItemViewModel(
    Guid Id,
    Guid? AmigoId,
    string Amigo,
    string Revista,
    int NumeroEdicao,
    DateTime DataEmprestimo,
    DateTime DataDevolucaoPrevista,
    DateTime? DataDevolucao,
    decimal Multa,
    StatusEmprestimo Status
);

public record ListarEmprestimosViewModel(
    Guid? AmigoId,
    string? NomeAmigo,
    List<EmprestimoListarItemViewModel> Emprestimos
);

public record CadastrarEmprestimoViewModel(
    [Required(ErrorMessage = "Selecione um amigo.")]
    Guid AmigoId,

    [Required(ErrorMessage = "Selecione uma revista.")]
    Guid RevistaId
);
