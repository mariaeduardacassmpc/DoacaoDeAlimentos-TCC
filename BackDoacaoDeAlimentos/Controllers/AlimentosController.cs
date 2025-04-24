using BackDoacaoDeAlimentos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackDoacaoDeAlimentos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlimentosController : ControllerBase
    {
        private static List<Alimento> _alimentos = new();

        [HttpGet]
        public ActionResult<List<Alimento>> Get()
        {
            return Ok(_alimentos);
        }

        [HttpPost]
        public ActionResult Post(Alimento alimento)
        {
            alimento.Id = _alimentos.Count > 0 ? _alimentos.Max(a => a.Id) + 1 : 1;
            _alimentos.Add(alimento);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var alimento = _alimentos.FirstOrDefault(a => a.Id == id);
            if (alimento == null)
                return NotFound();

            _alimentos.Remove(alimento);
            return NoContent();
        }
    }

}
