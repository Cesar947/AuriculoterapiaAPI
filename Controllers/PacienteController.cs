using Auriculoterapia.Api.Service;
using Microsoft.AspNetCore.Mvc;
using Auriculoterapia.Api.Domain;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController: ControllerBase
    {
        private IPacienteService PacienteService;

        public PacienteController(IPacienteService PacienteService){
            this.PacienteService = PacienteService;
        }


    

        [Authorize(Roles = "ESPECIALISTA")]
        [HttpGet]
        public IEnumerable<Paciente> FindAllByWord([FromQuery] string word){
            return PacienteService.buscarPorPalabra(word);
        }

        [Authorize(Roles = "ESPECIALISTA")]
        [HttpGet("lista")]
        public IEnumerable<Paciente> FindAll(){
            return PacienteService.FindAll();
        }



        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] Paciente paciente)
        {
            PacienteService.Save(paciente);
            if(paciente.Id != 0)
                return Ok(paciente); 
            else
                return BadRequest(new {message = "Correo inválido"}); 
        }

    }
}