using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using Auriculoterapia.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Auriculoterapia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisponibilidadController: ControllerBase
    {
        private IDisponibilidadService DisponibilidadService;

        public DisponibilidadController(IDisponibilidadService DisponibilidadService){
            this.DisponibilidadService = DisponibilidadService;
        }


        [HttpPost]
        public bool registrarDisponibilidad([FromBody] FormularioDisponibilidad entity, 
                                            [FromQuery] int especialistaId){
                
                return this.DisponibilidadService.registrarDisponibilidad(entity, especialistaId);
                
          }

        [HttpGet]
        public Disponibilidad listarDisponibilidadPorFecha([FromQuery] string fecha){
            return DisponibilidadService.listarPorFecha(fecha);
        }

        [HttpGet("horas")]
        public AvailabilityTimeRange listarHorasDisponibles([FromQuery] string fecha){
            return DisponibilidadService.listarHorasDisponibles(fecha);
        }

    }
}