using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
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

      

        public void Save(Evolucion entity)
        {
            throw new System.NotImplementedException();
        }

        public void saveByIdPaciente(Evolucion entity,int IdPaciente){
            evolucionRepository.saveByIdPaciente(entity,IdPaciente);
        }



    }
}