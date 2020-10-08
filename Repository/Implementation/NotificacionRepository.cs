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
                entity.Deshabilitado = false;
                //DateTime today = DateTime.Today;
                //DateTime hour = DateTime.Now;
                var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,myTimeZone);

                DateTime today= currentDateTime.Date;
                DateTime hour = currentDateTime;

                entity.FechaNotificación = today;
                entity.HoraNotificacion = hour;

                entity.Descripcion = getDescripcion(entity.TipoNotificacion);
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
                x.Deshabilitado == false).ToList();
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

            }

            
            return descripcion;
        }
    }
}