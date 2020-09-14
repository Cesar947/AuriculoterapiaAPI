using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;

namespace Auriculoterapia.Api.Service
{
    public interface IEvolucionService:IService<Evolucion>
    {

         void saveByIdPaciente(Evolucion entity,int IdPaciente);

         IEnumerable<Evolucion> getByIdPaciente_TipoTratamiento(string TipoTratamiento,int idPaciente);

    }
}