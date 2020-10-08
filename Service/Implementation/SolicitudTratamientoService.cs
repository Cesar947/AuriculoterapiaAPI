
using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository;

namespace Auriculoterapia.Api.Service.Implementation
{
    public class SolicitudTratamientoService : ISolicitudTratamientoService
    {

        private ISolicitudTratamientoRepository solicitudTratamientoRepository;

        private INotificacionRepository notificacionRepository;
        public SolicitudTratamientoService(ISolicitudTratamientoRepository solicitudTratamientoRepository,
        INotificacionRepository notificacionRepository)
        {
            this.solicitudTratamientoRepository = solicitudTratamientoRepository;
            this.notificacionRepository = notificacionRepository;
        }

        public IEnumerable<SolicitudTratamiento> FindAll()
        {
            return solicitudTratamientoRepository.FindAll();
        }



        public SolicitudTratamiento findByPacienteId(int pacienteId) {
            return solicitudTratamientoRepository.findByPacienteId(pacienteId);
        }

        public void Save(SolicitudTratamiento entity)
        {
            solicitudTratamientoRepository.Save(entity);

        }

        public void saveByUserId(SolicitudTratamiento entity, int userId) {
            solicitudTratamientoRepository.saveByUserId(entity, userId);
           

            if(entity.Id>0){
                var notificacion = new Notificacion();

                notificacion.EmisorId = userId;
                notificacion.ReceptorId = 1;
                notificacion.TipoNotificacion = "NUEVOTRATAMIENTO";

                notificacionRepository.Save(notificacion);
            }
        }

        public string obtenerImagenPorSolicitud(int solicitudId)
        {
            return solicitudTratamientoRepository.obtenerImagenPorSolicitud(solicitudId);
        }
    }
}