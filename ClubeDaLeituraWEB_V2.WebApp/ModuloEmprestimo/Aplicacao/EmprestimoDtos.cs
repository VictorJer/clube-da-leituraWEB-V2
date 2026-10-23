using ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Aplicacao;

public record EmprestimoListarDto(
    Guid Id,
    Guid? AmigoId,
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
    Guid? AmigoId,
    string? NomeAmigo,
    List<EmprestimoListarDto> Emprestimos
);

public record OpcaoEmprestimoDto(Guid Id, string Texto);

public record OpcoesEmprestimoDto(
    List<OpcaoEmprestimoDto> Amigos,
    List<OpcaoEmprestimoDto> Revistas
);

public record CadastrarEmprestimoDto(Guid AmigoId, Guid RevistaId);

public record DevolucaoEmprestimoDto(Guid? AmigoId, decimal Multa);
