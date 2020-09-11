using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Repository
{
    public interface IEvolucionRepository :IRepository<Evolucion> 
    {
        void saveByIdPaciente(Evolucion entity,int IdPaciente);
    }
}