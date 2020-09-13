
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using Auriculoterapia.Api.Service;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TratamientoController: ControllerBase
    {

        private ITratamientoService tratamientoService;

        public TratamientoController(ITratamientoService tratamientoService){
            this.tratamientoService = tratamientoService;
        }
        
        [HttpPost]
        public bool registrarTratamiento(FormularioTratamiento entity){
            return this.tratamientoService.registrarTratamiento(entity);
        }

        [HttpGet]
        public IEnumerable<Tratamiento> obtenerTratamientos(){
            return this.tratamientoService.FindAll();
        }

        [HttpGet("historial")]
        public IEnumerable<Tratamiento> obtenerTratamientosPorPaciente([FromQuery] int pacienteId) {

            return this.tratamientoService.listarPorPacienteId(pacienteId);

        }



    }
}