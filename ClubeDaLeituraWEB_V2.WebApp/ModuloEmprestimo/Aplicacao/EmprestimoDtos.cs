using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Aplicacao;

public record EmprestimoListarDto(
    string Id,
    string AmigoId,
    string Amigo,
    string Revista,
    int NumeroEdicao,
    DateTime DataEmprestimo,
    DateTime DataDevolucaoPrevista,
    DateTime? DataDevolucao,
    decimal Multa,
    StatusEmprestimo Status
);

public record ListarEmprestimosDto(
    string? AmigoId,
    string? NomeAmigo,
    List<EmprestimoListarDto> Emprestimos
);

public record OpcaoEmprestimoDto(string Id, string Texto);

public record OpcoesEmprestimoDto(
    List<OpcaoEmprestimoDto> Amigos,
    List<OpcaoEmprestimoDto> Revistas
);

public record CadastrarEmprestimoDto(string AmigoId, string RevistaId);

public record DevolucaoEmprestimoDto(string? AmigoId, decimal Multa);
