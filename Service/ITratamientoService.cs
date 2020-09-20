using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using System.Collections.Generic;

namespace Auriculoterapia.Api.Service
{
    public interface ITratamientoService: IService<Tratamiento>
    {
        bool registrarTratamiento(FormularioTratamiento t);
        IEnumerable<Tratamiento> listarPorPacienteId(int pacienteId);

        Tratamiento FindById(int id);
    }

}