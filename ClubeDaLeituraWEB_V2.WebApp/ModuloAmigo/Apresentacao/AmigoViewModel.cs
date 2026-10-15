using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Apresentacao;

public record ListarAmigosViewModel(
    string Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record CadastrarAmigoViewModel(
    [Required(ErrorMessage = "O nome do amigo deve ser preenchido.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do amigo deve conter entre 3 e 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O nome do responsável deve ser preenchido.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do responsável deve conter entre 3 e 100 caracteres.")]
    string NomeResponsavel,

    [Required(ErrorMessage = "O telefone deve ser preenchido.")]
    [StringLength(20, ErrorMessage = "O telefone deve conter no máximo 20 caracteres.")]
    string Telefone
);

public record EditarAmigoViewModel(
    string Id,

    [Required(ErrorMessage = "O nome do amigo deve ser preenchido.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do amigo deve conter entre 3 e 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O nome do responsável deve ser preenchido.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do responsável deve conter entre 3 e 100 caracteres.")]
    string NomeResponsavel,

    [Required(ErrorMessage = "O telefone deve ser preenchido.")]
    [StringLength(20, ErrorMessage = "O telefone deve conter no máximo 20 caracteres.")]
    string Telefone
);

public record ExcluirAmigoViewModel(
    string Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);
