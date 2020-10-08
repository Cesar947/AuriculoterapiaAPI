using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Auriculoterapia.Api.Controllers
{
    [Route("api/[controller]")]
    public class NotificacionController : ControllerBase
    {
        private INotificacionService notificacionService;

        public NotificacionController(INotificacionService notificacionService){
            this.notificacionService = notificacionService;
        }

        [HttpPost]
        public IActionResult Save([FromBody] Notificacion notificacion){
            notificacionService.Save(notificacion);
            return Ok(notificacion);
        }

        [HttpGet("{receptorId}")]
        public IActionResult FindAllNotificacionByReceptorId(int receptorId){
            var notificaciones = notificacionService.getNotificacionByReceptorId(receptorId);

            if(notificaciones == null){
                return BadRequest(new {message = "No tiene ninguna notificacion"});
            }else{
                return Ok(notificaciones);
            }
        }

        [HttpPut("deshabilitar/{id}")]
        public IActionResult ModificarDeshabilitarNotificacion(int id){

            var estadoNotificacion = notificacionService.modificarDeshabilitar(id);

            if(estadoNotificacion == false){
                return BadRequest(new {message = "Hubo un problema, no se encontro la notificacion"});
            }else{
                return Ok(estadoNotificacion);
            }
        }
    }
}