using System.ComponentModel.DataAnnotations;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Dominio;

public enum StatusRevista
{
    [Display(Name = "Disponível")]
    Disponivel,

    [Display(Name = "Emprestada")]
    Emprestada,

    [Display(Name = "Reservada")]
    Reservada
}
