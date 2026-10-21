using AutoMapper;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;

public class CaixaProfile : Profile
{
    public CaixaProfile()
    {
        CreateMap<ListarCaixasDto, ListarCaixasViewModel>();
    }

}