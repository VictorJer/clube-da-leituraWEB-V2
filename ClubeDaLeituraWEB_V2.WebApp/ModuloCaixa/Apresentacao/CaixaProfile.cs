using AutoMapper;
using ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Aplicacao;

namespace ClubeDaLeituraWEB_V2.WebApp.ModuloCaixa.Apresentacao;

public class CaixaProfile : Profile
{
    public CaixaProfile()
    {
        CreateMap<ListarCaixasDto, ListarCaixasViewModel>();
        CreateMap<CadastrarCaixaViewModel, CadastrarCaixaDto>();
        CreateMap<EditarCaixaViewModel, EditarCaixaDto>();
        CreateMap<DetalhesCaixaDto, CadastrarCaixaViewModel>();
        CreateMap<DetalhesCaixaDto, EditarCaixaViewModel>();
        CreateMap<DetalhesCaixaDto, ExcluirCaixaViewModel>();
    }

}