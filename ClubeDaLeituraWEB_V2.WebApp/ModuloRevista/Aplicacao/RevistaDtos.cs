using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;

public record ListarRevistasDto(
    Guid Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string EtiquetaCaixa,
    StatusRevista Status
);

public record CadastrarRevistaDto(
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    Guid CaixaId,
    StatusRevista Status
);

public record EditarRevistaDto(
    Guid Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    Guid CaixaId,
    StatusRevista Status
);

public record DetalhesRevistaDto(
    Guid Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    Guid CaixaId,
    string EtiquetaCaixa,
    StatusRevista Status
);
