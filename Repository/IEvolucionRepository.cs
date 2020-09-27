using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using AuriculoterapiaAPI.Helpers;

namespace Auriculoterapia.Api.Repository
{
    public interface IEvolucionRepository :IRepository<Evolucion> 
    {
        void saveByIdPaciente(Evolucion entity,int IdPaciente);

        IEnumerable<Evolucion> getByIdPaciente_TipoTratamiento(string TipoTratamiento,int idPaciente);

        IEnumerable<ResponseResultsPatient> getByIdPaciente_TipoTratamiento_Results(string TipoTratamiento, int idPaciente);

      
    }
}