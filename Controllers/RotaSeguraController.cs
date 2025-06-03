using Microsoft.AspNetCore.Mvc;
using Sessions_app.Models;
using Sessions_app.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sessions_app.Controllers
{
    [ApiController]
    [Route("api/rotas-seguras")]
    public class RotaSeguraController : ControllerBase
    {
        private readonly RotaSeguraService _service;
        private readonly IUrlHelper _urlHelper;
        private readonly LinkGenerator _linkGenerator;


        public RotaSeguraController(
            RotaSeguraService service,
            IUrlHelper urlHelper,
            LinkGenerator linkGenerator)
        {
            _service = service;
            _urlHelper = urlHelper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id}", Name = "GetRotaSeguraById")]
        public async Task<ActionResult<RotaSegura>> Get(int id)
        {
            var rota = await _service.GetByIdAsync(id);
            if (rota == null)
            {
                return NotFound();
            }

            // HATEOAS
            rota.Links = new List<Link>
            {
                new Link(_linkGenerator.GetPathByName("GetRotaSeguraById", new { id }), "self", "GET"),
                new Link(_linkGenerator.GetPathByName("DeleteRotaSegura", new { id }), "delete_rota", "DELETE"),
                new Link(_linkGenerator.GetPathByName("UpdateRotaSegura", new { id }), "update_rota", "PUT")
            };

            return Ok(rota);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RotaSegura>>> GetAll()
        {
            var rotas = await _service.GetAllAsync();
            return Ok(rotas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RotaSegura rota)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddAsync(rota);
            return CreatedAtAction(nameof(Get), new { id = rota.Id }, rota);
        }

        [HttpPut("{id}", Name = "UpdateRotaSegura")]
        public async Task<IActionResult> Put(int id, [FromBody] RotaSegura rota)
        {
            if (id != rota.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateAsync(rota);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteRotaSegura")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}