using System;

using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using Auriculoterapia.Api.Service;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Auriculoterapia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitaController : ControllerBase
    {
        private ICitaService CitaService;

        public CitaController(ICitaService CitaService){
            this.CitaService = CitaService;
        }

        [HttpPost("especialista")]
        public FormularioCita RegistrarCitaEspecialista([FromBody] FormularioCita entity, [FromQuery] int PacienteId){
            CitaService.RegistrarCita(entity, PacienteId);
            return entity;
        
        }

        [HttpGet("paciente")]
        [Authorize(Roles = "PACIENTE")]
        public ActionResult ListarCitasPorUsuarioId([FromQuery] int usuarioId){
            return Ok(this.CitaService.listarCitasPorUsuarioId(usuarioId));
        }

        [HttpPost("paciente")]
        public FormularioCitaPaciente RegistrarCitaPaciente([FromBody] FormularioCitaPaciente entity, [FromQuery] int PacienteId){
            CitaService.RegistrarCitaPaciente(entity, PacienteId);
            Console.WriteLine(StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created));
             return entity;
        }

        [HttpGet]
        public ActionResult ListarCitas(){
            return Ok(CitaService.FindAll());
        }

        [HttpGet("hello_world")]
        public string Get()
        {
            return "Hello World!";
        }

        [HttpPut("estado")]
        public bool actualizarEstadoCita([FromQuery] int citaId, [FromQuery] string estado, [FromQuery] int usuarioId){
            return CitaService.actualizarEstadoCita(citaId, estado, usuarioId);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "PACIENTE")]
        public Cita findByIdParaPaciente([FromRoute] int id){
            return CitaService.findByIdParaPaciente(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "PACIENTE")]
        public bool actualizarCita([FromRoute] int id, FormularioCitaPaciente form){
            return CitaService.actualizarCita(id, form);
        }

    }
}
