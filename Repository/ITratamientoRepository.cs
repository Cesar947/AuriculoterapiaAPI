using Auriculoterapia.Api.Domain;
using System.Collections.Generic;

namespace Auriculoterapia.Api.Repository
{
    public interface ITratamientoRepository: IRepository<Tratamiento>
    {
        IEnumerable<Tratamiento> listarPorPacienteId(int pacienteId);
    }
}