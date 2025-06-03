using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sessions_app.Models;
using Sessions_app.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sessions_app.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;
        private readonly IUrlHelper _urlHelper;
        private readonly LinkGenerator _linkGenerator;

        public UsuarioController(
            UsuarioService service,
            IUrlHelper urlHelper,
            LinkGenerator linkGenerator)
        {
            _service = service;
            _urlHelper = urlHelper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id}", Name = "GetUsuarioById")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // HATEOAS
            usuario.Links = new List<Link>
            {
                    new Link(_linkGenerator.GetPathByName("GetUsuarioById", new { id }), "self", "GET"),
                    new Link(_linkGenerator.GetPathByName("DeleteUsuario", new { id }), "delete_usuario", "DELETE"),
                    new Link(_linkGenerator.GetPathByName("UpdateUsuario", new { id }), "update_usuario", "PUT")
            };

            return Ok(usuario);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            var usuarios = await _service.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddAsync(usuario);
            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}", Name = "UpdateUsuario")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteUsuario")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}