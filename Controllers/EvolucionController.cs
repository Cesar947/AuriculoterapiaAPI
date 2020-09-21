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

        /*[HttpGet("{idPaciente}/{TipoTratamiento}")]
        public IActionResult getByIdPaciete_TipoTratamiento(string TipoTratamiento,int idPaciente){
            var evoluciones = evolucionService.getByIdPaciente_TipoTratamiento(TipoTratamiento,idPaciente);
            return Ok(evoluciones);
        }*/

        [HttpGet("{idPaciente}/{TipoTratamiento}")]
        public IActionResult getByIdPaciete_TipoTratamiento2(string TipoTratamiento,int idPaciente){
            var evoluciones = evolucionService.getByIdPaciente_TipoTratamiento_Results(TipoTratamiento,idPaciente);
            return Ok(evoluciones);
        }

    

    }
}