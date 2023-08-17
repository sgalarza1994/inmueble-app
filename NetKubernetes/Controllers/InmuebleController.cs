using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Inmuebles;
using NetKubernetes.Dtos;
using NetKubernetes.Middlware;
using NetKubernetes.Models;

namespace NetKubernetes.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InmuebleController : ControllerBase
{
    private readonly IInmuebleRepository _inmuebleRepository;
    private IMapper _mapper;
    public InmuebleController(IInmuebleRepository inmuebleRepository,IMapper mapper)
    {
        _inmuebleRepository = inmuebleRepository;
        _mapper  = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<InmuebleResponseDto>> GetInmuebles()
    {
        var inmeubles = _inmuebleRepository.GetAllInmuebles();
        return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmeubles));
    }
    [HttpGet("{id}",Name = "GetInmueblesById")]
    public ActionResult<InmuebleResponseDto> GetInmueblesById(int id)
    {
        var inmeubles = _inmuebleRepository.GetInmuebleById(id);
        if(inmeubles is null)
            throw new MiddlwareException(System.Net.HttpStatusCode.NotFound , new {mensaje = "No se encontro el inmueble con ese id "});
        return Ok(_mapper.Map<InmuebleResponseDto>(inmeubles));
    }
    [HttpPost]
    public async Task<ActionResult<InmuebleResponseDto>> CrearInmueble(InmuebleRequestDto request)
    {
        var inmueble = _mapper .Map<Inmueble>(request);
         _inmuebleRepository.CreateInmueble(inmueble);
         _inmuebleRepository.SaveChanges();
        return CreatedAtRoute(nameof(GetInmueblesById), new {inmueble.Id},inmueble);
    }
    
}