using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using Auriculoterapia.Api.Repository;
using AuriculoterapiaAPI.Helpers;

namespace Auriculoterapia.Api.Service.Implementation
{
    public class EvolucionService : IEvolucionService
    {
        private IEvolucionRepository evolucionRepository;
        private IPacienteRepository pacienteRepository;
        private INotificacionRepository notificacionRepository;

        public EvolucionService(IEvolucionRepository evolucionRepository,
        IPacienteRepository pacienteRepository,
        INotificacionRepository notificacionRepository){
            this.evolucionRepository = evolucionRepository;
            this.pacienteRepository = pacienteRepository;
            this.notificacionRepository = notificacionRepository;
        }
        
        public IEnumerable<Evolucion> FindAll()
        {
            throw new System.NotImplementedException();
        }


        public IEnumerable<Evolucion> getByIdPaciente_TipoTratamiento(string TipoTratamiento,int idPaciente){
            return evolucionRepository.getByIdPaciente_TipoTratamiento(TipoTratamiento,idPaciente);
        }


        public void Save(Evolucion entity)
        {
            throw new System.NotImplementedException();
        }

        public void saveByIdPaciente(Evolucion entity,int IdPaciente){
            evolucionRepository.saveByIdPaciente(entity,IdPaciente);

             if(entity.Id>0){
                var notificacion = new Notificacion();

                var emisor = pacienteRepository.FindById(IdPaciente); 

                notificacion.EmisorId = emisor.UsuarioId;
                notificacion.ReceptorId = 1;
                notificacion.TipoNotificacion = "REGISTRARFORMULARIOEVOLUCION";

                notificacionRepository.saveNotificacion(notificacion,entity.TipoTratamiento);
            }
        }

        public IEnumerable<ResponseResultsPatient> getByIdPaciente_TipoTratamiento_Results(string TipoTratamiento, int idPaciente){
            return evolucionRepository.getByIdPaciente_TipoTratamiento_Results(TipoTratamiento,idPaciente);
        }

    }
}