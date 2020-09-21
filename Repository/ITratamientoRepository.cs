using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using System.Collections.Generic;

namespace Auriculoterapia.Api.Repository
{
    public interface ITratamientoRepository: IRepository<Tratamiento>
    {
        IEnumerable<Tratamiento> listarPorPacienteId(int pacienteId);


    }
}