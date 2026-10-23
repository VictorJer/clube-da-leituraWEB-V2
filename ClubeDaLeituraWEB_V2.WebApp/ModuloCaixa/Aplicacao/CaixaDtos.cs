namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;

public record ListarCaixasDto(
    Guid Id,
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);

public record CadastrarCaixaDto(
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);

public record EditarCaixaDto(
    Guid Id,
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);

public record DetalhesCaixaDto(
    Guid Id,
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);