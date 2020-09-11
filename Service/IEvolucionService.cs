using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Service
{
    public interface IEvolucionService:IService<Evolucion>
    {
         void saveByIdPaciente(Evolucion entity,int IdPaciente);
    }
}