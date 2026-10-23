namespace ClubeDaLeituraWEB_V2.WebApp.ModuloAmigo.Aplicacao;

public record ListarAmigosDto(
    Guid Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record CadastrarAmigoDto(
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record EditarAmigoDto(
    Guid Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);

public record DetalhesAmigoDto(
    Guid Id,
    string Nome,
    string NomeResponsavel,
    string Telefone
);