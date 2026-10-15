using System.ComponentModel.DataAnnotations;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Apresentacao;

public record ListarRevistasViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string Caixa,
    StatusRevista Status
);

public record CadastrarRevistaViewModel(
    [Required(ErrorMessage = "O título deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Range(1, int.MaxValue, ErrorMessage = "O número da edição deve ser maior que zero.")]
    int NumeroEdicao,

    [Required(ErrorMessage = "A data de publicação deve ser preenchida.")]
    [DataType(DataType.Date)]
    DateTime AnoPublicacao,

    [Required(ErrorMessage = "Selecione uma caixa.")]
    string CaixaId,

    StatusRevista Status
);

public record EditarRevistaViewModel(
    [Required]
    string Id,

    [Required(ErrorMessage = "O título deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Range(1, int.MaxValue, ErrorMessage = "O número da edição deve ser maior que zero.")]
    int NumeroEdicao,

    [Required(ErrorMessage = "A data de publicação deve ser preenchida.")]
    [DataType(DataType.Date)]
    DateTime AnoPublicacao,

    [Required(ErrorMessage = "Selecione uma caixa.")]
    string CaixaId,

    StatusRevista Status
);

public record ExcluirRevistaViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string Caixa,
    StatusRevista Status
);
