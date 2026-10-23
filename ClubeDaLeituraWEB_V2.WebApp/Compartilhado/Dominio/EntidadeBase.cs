using System.Security.Cryptography;

namespace ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Dominio;

public abstract class EntidadeBase<T>
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public abstract List<string> Validar();
    public abstract void AtualizarDados(T entidadeAtualizada);
}