using System.Collections.Generic;
using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Repository
{
    public interface IEvolucionRepository :IRepository<Evolucion> 
    {
        void saveByIdPaciente(Evolucion entity,int IdPaciente);

        IEnumerable<Evolucion> getByIdPaciente_TipoTratamiento(string TipoTratamiento,int idPaciente);
    }
}