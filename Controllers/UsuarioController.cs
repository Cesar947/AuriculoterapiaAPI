using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Service;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Auriculoterapia.Api.Helpers;

namespace Auriculoterapia.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioService usuarioService;

        public UsuarioController(IUsuarioService usuarioService){
            this.usuarioService = usuarioService;
        }


        [Authorize(Roles = "ESPECIALISTA")]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(
                usuarioService.FindAll()
            );
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            usuarioService.Save(usuario);
            return Ok(usuario); 
        }

        [AllowAnonymous]
        [HttpPost("autenticar")]
        public IActionResult Autenticar([FromBody] Usuario usuario)
        {
            var user = usuarioService.Autenticar(usuario.NombreUsuario,usuario.Contrasena);

            if (user == null){
                return BadRequest(new {message = "Nombre de usuario o contraseña incorrectos"});    
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("actualizarContrasena")]
        public IActionResult actualizar_Contrasena([FromBody] Usuario usuario){
            var nuevaContrasena = usuarioService.actualizar_Contrasena(usuario.NombreUsuario,usuario.PalabraClave,usuario.Contrasena);

            if(nuevaContrasena == null){
                return BadRequest(new {message = "Nombre de usuario o palabra clave incorrectos"});
            }
            if(nuevaContrasena.Contrasena == "SAME"){
                return BadRequest(new {message = "La contraseña nueva es la misma que la actual, volver a modificar la contraseña"});
            }

            if(nuevaContrasena.Contrasena == "EXISTENTE"){
                return BadRequest(new {message = "La contraseña nueva ya existe, volver a modificar la contraseña"});
            }

            if(nuevaContrasena.Contrasena == "SAMEKEYWORD"){
                return BadRequest(new {message = "La contraseña nueva es la misma que la palabra clave, volver a modificar la contraseña"});
            }

            if(nuevaContrasena.Contrasena == "SAMEUSER"){
                return BadRequest(new {message = "La contraseña nueva es la misma que el nombre de usuario, volver a modificar la contraseña"});
            }


            return Ok(nuevaContrasena);

        }

      
        
        /*[HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            var user = usuarioService.FinbyId(id);

            if(user == null){
                return NotFound();
            }else{

                return Ok(user);
            }           

        }*/

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            var user = usuarioService.BuscarPorId(id);

            if(user == null){
                return BadRequest(new {message = "Hubo un problema, por favor inténtelo nuevamente"});
            }else{
                return Ok(user);
            }           

        }

        [AllowAnonymous]
        [HttpPost("actualizarpalabraclave")]
        public IActionResult ActualizarKeyword([FromBody] ResponseActualizarKeyWord response)
        {
            var user = usuarioService.Actualizar_KeyWord(response.Id,response.PalabraClave,response.NuevaPalabraClave);

            if(user == null){
                return BadRequest(new {message = "Palabra clave actual incorrecta"});
            }else{
                if(user.PalabraClave == "SAME"){
                return BadRequest(new {message = "Palabra clave nueva se repite con la actual"});
                }
                if(user.PalabraClave == "SAMEUSER"){
                    return BadRequest(new {message = "Palabra clave tiene que ser diferente al nombre de usuario"});
                }
                return Ok(user);
            }           

        }

        [AllowAnonymous]
        [HttpPost("actualizarfoto")]
        public IActionResult ActualizarFoto([FromBody] ResponseActualizarFoto response)
        {
            var user = usuarioService.Actualizar_Foto(response.Id,response.Foto);

            if(user == null){
                return BadRequest(new {message = "No se pudo actualizar la foto"});
            }else{
                return Ok(user);
            }   
        }

        [AllowAnonymous]
        [HttpGet("foto/{id}")]
        public IActionResult BuscarFotoPorUserId(int id)
        {
            var user = usuarioService.Buscar_Foto(id);

            if(user == null){
                return BadRequest(new {message = "Hubo un problema, por favor inténtelo nuevamente"});
            }else{
                return Ok(user);
            }           

        }
 
        
    }
}