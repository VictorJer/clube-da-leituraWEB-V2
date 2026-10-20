namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;

public record ListarCaixasDto(
    string Id,
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
    string Id,
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);

public record DetalhesCaixaDto(
    string Id,
    string Etiqueta,
    string Cor,
    int DiasDeEmprestimo
);