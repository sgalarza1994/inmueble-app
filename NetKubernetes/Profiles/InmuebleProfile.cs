using AutoMapper;
using NetKubernetes.Dtos;
using NetKubernetes.Models;

namespace NetKubernetes.Profiles;

public class InmuebleProfile : Profile
{
    public InmuebleProfile()
    {
        CreateMap<Inmueble,InmuebleResponseDto>().ReverseMap();
        CreateMap<InmuebleRequestDto,Inmueble>().ReverseMap();
    }
}