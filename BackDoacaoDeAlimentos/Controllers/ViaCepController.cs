using BackDoacaoDeAlimentos.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/viacep")]
public class ViaCepController : ControllerBase
{
    private readonly ViaCepService _viaCepService;

    public ViaCepController(ViaCepService viaCepService)
    {
        _viaCepService = viaCepService;
    }

    [HttpGet("{cep}")]
    public async Task<IActionResult> BuscarCep(string cep)
    {
        try
        {
            var resultado = await _viaCepService.BuscarEnderecoPorCep(cep);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}
