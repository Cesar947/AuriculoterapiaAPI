using System.Collections.Generic;

using Auriculoterapia.Api.Domain;
namespace Auriculoterapia.Api.Repository
{
    public interface ICitaRepository: IRepository<Cita>
    {
        IEnumerable<Cita> listarCitasPorUsuarioId(int usuarioId);

        bool actualizarEstadoCita(int citaId, string estado);

        bool actualizarCita(Cita entity, int id);
    }
}
