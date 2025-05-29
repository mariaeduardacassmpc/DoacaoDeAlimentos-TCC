using BackDoacaoDeAlimentos.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ViaCepController : ControllerBase
{
    private readonly ViaCepService _viaCepService;

    public ViaCepController(ViaCepService viaCepService)
    {
        _viaCepService = viaCepService;
    }

    [HttpGet("BuscarCep/{cep}")]
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
