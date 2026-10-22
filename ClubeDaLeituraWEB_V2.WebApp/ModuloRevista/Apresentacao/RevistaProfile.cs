using AutoMapper;
using ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Aplicacao;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloRevista.Apresentacao;

public class RevistaProfile : Profile
{
    public RevistaProfile()
    {
        CreateMap<ListarRevistasDto, ListarRevistasViewModel>();
        CreateMap<CadastrarRevistaViewModel, CadastrarRevistaDto>();
    }
}