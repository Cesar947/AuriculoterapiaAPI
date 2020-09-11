using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuriculoterapiaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvolucionController: ControllerBase
    {
         private IEvolucionService evolucionService;

         public EvolucionController(IEvolucionService evolucionService){
             this.evolucionService = evolucionService;
         }

        [HttpPost("{IdPaciente}")]
        public IActionResult PostByIdPatient([FromBody] Evolucion evolucion, int IdPaciente)
        {
            evolucionService.saveByIdPaciente(evolucion,IdPaciente);
            if(evolucion.Id != 0)
                return Ok(evolucion); 
            else
                return BadRequest(new {message = "error en el registro, vuelva a intentar"}); 
        }
    }
}