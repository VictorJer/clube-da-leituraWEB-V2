using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;

public record ListarCaixasViewModel(
    string Id,
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);

public record CadastrarCaixaViewModel(
    [Required(ErrorMessage = "O campo \"Etiqueta\" deve ser preenchido.")]
    [StringLength(50, ErrorMessage = "O campo \"Etiqueta\" deve conter no máximo 50 caracteres.")]
    string Etiqueta,

    [Required(ErrorMessage = "O campo \"Cor\" deve ser preenchido.")]
    string Cor,

    [Range(1, int.MaxValue, ErrorMessage = "O campo \"Dias de Empréstimo\" deve conter um valor maior que 0.")]
    int DiasDeEmprestimo
);

public record EditarCaixaViewModel(
    string Id,

    [Required(ErrorMessage = "O campo \"Etiqueta\" deve ser preenchido.")]
    [StringLength(50, ErrorMessage = "O campo \"Etiqueta\" deve conter no máximo 50 caracteres.")]
    string Etiqueta,

    [Required(ErrorMessage = "O campo \"Cor\" deve ser preenchido.")]
    string Cor,

    [Range(1, int.MaxValue, ErrorMessage = "O campo \"Dias de Empréstimo\" deve conter um valor maior que 0.")]
    int DiasDeEmprestimo
);

public record ExcluirCaixaViewModel(
    string Id,
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);