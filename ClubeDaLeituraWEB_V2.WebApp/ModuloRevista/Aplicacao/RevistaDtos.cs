using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;

public record ListarRevistasDto(
    string Id,
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
    string CaixaId,
    StatusRevista Status
);

public record EditarRevistaDto(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaId,
    StatusRevista Status
);

public record DetalhesRevistaDto(
    string Id,
    string Titulo,
    int NumeroEdicao,
    DateTime AnoPublicacao,
    string CaixaId,
    StatusRevista Status
);
