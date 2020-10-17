using System;
using System.Collections.Generic;
using System.Linq;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;

namespace Auriculoterapia.Api.Repository.Implementation
{
    public class NotificacionRepository : INotificacionRepository
    {
        private ApplicationDbContext context;

        public NotificacionRepository(ApplicationDbContext context){
            this.context = context;
        }
        
        public IEnumerable<Notificacion> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Notificacion FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(Notificacion entity)
        {
            try{

                var usuarioEmisor = this.context.Usuarios.FirstOrDefault(u => u.Id == entity.EmisorId);

                var nombreCompleto = $"{usuarioEmisor.Nombre} {usuarioEmisor.Apellido}";

                Console.WriteLine($"Título: {nombreCompleto}");

                entity.Deshabilitado = false;
                entity.Leido = false;
                //DateTime today = DateTime.Today;
                //DateTime hour = DateTime.Now;
                var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,myTimeZone);

                DateTime today= currentDateTime.Date;
                DateTime hour = currentDateTime;

                entity.FechaNotificacion = today;
                entity.HoraNotificacion = hour;
                
                
                entity.Titulo = nombreCompleto;
                entity.Descripcion = getDescripcion(entity.TipoNotificacion);
                context.Notificaciones.Add(entity);
                context.SaveChanges();

            }catch(System.Exception){
                throw;
            }
        }

        public void saveNotificacion(Notificacion entity,string TipoTratamiento){
             try{

                var usuarioEmisor = this.context.Usuarios.FirstOrDefault(u => u.Id == entity.EmisorId);

                var nombreCompleto = $"{usuarioEmisor.Nombre} {usuarioEmisor.Apellido}";

                Console.WriteLine($"Título: {nombreCompleto}");

                entity.Deshabilitado = false;
                entity.Leido = false;
                //DateTime today = DateTime.Today;
                //DateTime hour = DateTime.Now;
                var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,myTimeZone);

                DateTime today= currentDateTime.Date;
                DateTime hour = currentDateTime;

                entity.FechaNotificacion = today;
                entity.HoraNotificacion = hour;
                
                
                entity.Titulo = nombreCompleto;
                entity.Descripcion = getDescripcion(entity.TipoNotificacion);

                if(TipoTratamiento != null && entity.TipoNotificacion == "REGISTRARFORMULARIOEVOLUCION"){
                    entity.Descripcion = $"{entity.Descripcion} para {TipoTratamiento}";
                }

                context.Notificaciones.Add(entity);
                context.SaveChanges();

            }catch(System.Exception){
                throw;
            }
        }


        public  IEnumerable<Notificacion> getNotificacionByReceptorId(int receptorId){
            var notificaciones = new List<Notificacion>();
            try{
                notificaciones = context.Notificaciones
                .Where(x => x.ReceptorId == receptorId && 
                x.Deshabilitado == false).OrderByDescending(n => n.Id).ToList();
            }catch(System.Exception)
            {
                throw;
            }
            if(notificaciones.Count == 0){
                return null;
            }else{
                return notificaciones;
            }
        }

        public bool modificarDeshabilitar(int id){
            var notificacion = context.Notificaciones.FirstOrDefault(x => x.Id == id);

            if(notificacion != null){
                notificacion.Deshabilitado = true;  
                context.SaveChanges();
                return true;
            }
            else{
                return false;
            }
            
        }

        public int numeroDeNotificacionesPorReceptorId(int id){
            var count = 0;
            try{
                count = this.context.Notificaciones.Where(n => n.ReceptorId == id
                 && n.Deshabilitado == false 
                 && n.Leido == false).Count();
            } catch(Exception e){
                throw;
            }
            return count;
        }

        public bool leerNotificacionesPorReceptorId(int receptorId){
            var leidos = false;
            try{
                var notificaciones = this.context.Notificaciones.Where(n => n.ReceptorId == receptorId
                && n.Leido == false && n.Deshabilitado == false);

                foreach(var n in notificaciones){
                    n.Leido = true;
                }
                leidos = true;
                this.context.SaveChanges();
            } catch(Exception e){
                throw;
            }
            return leidos;
        }


        private string getDescripcion(string TipoNotificacion){
            var descripcion = "";

            switch(TipoNotificacion){
                case "NUEVACITA":
                    descripcion = "Acaba de registrar una nueva cita";
                break;

                case "MODIFICARCITA":
                    descripcion = "Acaba de modificar una cita";
                break;

                case "CANCELARCITA":
                    descripcion = "Acaba de cancelar una cita";
                break;

                case "NUEVOTRATAMIENTO":
                    descripcion = "Acaba de registrar una solicitud de tratamiento";
                break;


                case "RESPONDERTRATAMIENTO":
                    descripcion = "Acaba de responder a la solicitud de tratamiento";
                break;

                case "CANCELARTRATAMIENTO":
                    descripcion = "Acaba de cancelar la solicitud de tratamiento";
                break;

                case "REGISTRARFORMULARIOEVOLUCION":
                    descripcion = "Acaba de registrar el formulario de evolución";
                break;

            }

            
            return descripcion;
        }

    }
}