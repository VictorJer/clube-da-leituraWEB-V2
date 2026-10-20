using ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Dominio;

namespace ClubeDaLeituraWEB_V2.WebApp.Compartilhado.Infra;

public interface IRepositorio<T> where T : EntidadeBase<T>
{
    void Cadastrar(T entidade);
    bool Editar(string idSelecionado, T entidadeAtualizada);
    bool Excluir(string idSelecionado);
    T? SelecionarPorId(string idSelecionado);
    List<T> SelecionarTodos();
    List<T> Filtrar(Predicate<T> filtro);
}