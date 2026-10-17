using System.ComponentModel.DataAnnotations;
using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Apresentacao;

public record EmprestimoListarItemViewModel(
    string Id,
    string AmigoId,
    string Amigo,
    string Revista,
    int NumeroEdicao,
    DateTime DataEmprestimo,
    DateTime DataDevolucaoPrevista,
    DateTime? DataDevolucao,
    StatusEmprestimo Status
);

public record ListarEmprestimosViewModel(
    string? AmigoId,
    string? NomeAmigo,
    List<EmprestimoListarItemViewModel> Emprestimos
);

public record CadastrarEmprestimoViewModel(
    [Required(ErrorMessage = "Selecione um amigo.")]
    string AmigoId,

    [Required(ErrorMessage = "Selecione uma revista.")]
    string RevistaId
);
