using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository;

namespace Auriculoterapia.Api.Service.Implementation
{
    public class EvolucionService : IEvolucionService
    {
        private IEvolucionRepository evolucionRepository;

        public EvolucionService(IEvolucionRepository evolucionRepository){
            this.evolucionRepository = evolucionRepository;
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
        }
    }
}