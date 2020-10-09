using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository;

namespace Auriculoterapia.Api.Service.Implementation
{
    public class NotificacionService : INotificacionService
    {
        private INotificacionRepository NotificacionRepository;

        public NotificacionService(INotificacionRepository NotificacionRepository){
            this.NotificacionRepository = NotificacionRepository;
        }

        public IEnumerable<Notificacion> FindAll()
        {
            return NotificacionRepository.FindAll();
        }

        public void Save(Notificacion entity)
        {
            NotificacionRepository.Save(entity);
        }

        public IEnumerable<Notificacion> getNotificacionByReceptorId(int receptorId){
            return NotificacionRepository.getNotificacionByReceptorId(receptorId);
        }

        public bool modificarDeshabilitar(int id){
            return NotificacionRepository.modificarDeshabilitar(id);
        }

        public int numeroDeNotificacionesPorReceptorId(int id){
            return NotificacionRepository.numeroDeNotificacionesPorReceptorId(id);
        }

        public bool leerNotificacionesPorReceptorId(int receptorId){
            return NotificacionRepository.leerNotificacionesPorReceptorId(receptorId);
        }
    }
}