using Auriculoterapia.Api.Service;
using Microsoft.AspNetCore.Mvc;
using Auriculoterapia.Api.Domain;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Auriculoterapia.Api.Helpers;
using System;

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
           
            if(paciente.Usuario.Id ==0){
                return BadRequest(new{message = "Usuario o contraseña registrados"});
            }
            if(paciente.Usuario.Id == -1){
                return BadRequest(new{message = "Este correo no se encuentra disponible"});
            }
            
            if(paciente.Id != 0)
                return Ok(paciente); 
            else
                return BadRequest(new {message = "Correo inválido"}); 
        }

        [AllowAnonymous]
        [HttpGet("pacientesPorSexo")]
        public CantidadPacientesPorSexo retornarPacientesPorSexo([FromQuery] string tratamiento){
            return this.PacienteService.retornarPacientesPorSexo(tratamiento);
        }

    }
}