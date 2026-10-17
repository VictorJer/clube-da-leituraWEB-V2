using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloEmprestimo.Dominio;

public enum StatusEmprestimo
{
    [Display(Name = "Aberto")]
    Aberto,

    [Display(Name = "Concluído")]
    Concluido,

    [Display(Name = "Atrasado")]
    Atrasado
}
