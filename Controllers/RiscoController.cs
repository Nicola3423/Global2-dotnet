using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sessions_app.Models;
using Sessions_app.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sessions_app.Controllers
{
    [ApiController]
    [Route("api/riscos")]
    public class RiscoController : ControllerBase
    {
        private readonly RiscoService _service;
        private readonly IUrlHelper _urlHelper;
        private readonly LinkGenerator _linkGenerator;

        public RiscoController(
            RiscoService service,
            IUrlHelper urlHelper,
            LinkGenerator linkGenerator)
        {
            _service = service;
            _urlHelper = urlHelper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id}", Name = "GetRiscoById")]
        public async Task<ActionResult<Risco>> Get(int id)
        {
            var risco = await _service.GetByIdAsync(id);
            if (risco == null)
            {
                return NotFound();
            }

            // HATEOAS
            risco.Links = new List<Link>
            {
                new Link(_linkGenerator.GetPathByName("GetRiscoById", new { id }), "self", "GET"),
                new Link(_linkGenerator.GetPathByName("DeleteRisco", new { id }), "delete_risco", "DELETE"),
                new Link(_linkGenerator.GetPathByName("UpdateRisco", new { id }), "update_risco", "PUT")
            };

            return Ok(risco);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Risco>>> GetAll()
        {
            var riscos = await _service.GetAllAsync();
            return Ok(riscos);
        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Post([FromBody] Risco risco)
        {

            if (risco.Nivel == 0)
            {
                ModelState.Remove("Nivel");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddAsync(risco);
            return CreatedAtAction(nameof(Get), new { id = risco.Id }, risco);
        }

        [HttpPut("{id}", Name = "UpdateRisco")]
        public async Task<IActionResult> Put(int id, [FromBody] Risco risco)
        {
            if (id != risco.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateAsync(risco);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteRisco")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
