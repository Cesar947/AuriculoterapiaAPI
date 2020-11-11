using System;
using Auriculoterapia.Api.Domain;
using System.Collections.Generic;
using Auriculoterapia.Api.Helpers;


namespace Auriculoterapia.Api.Service
{
    public interface IPacienteService: IService<Paciente>
    {
        IEnumerable<Paciente> buscarPorPalabra(string palabra);

        PacienteResultsParameters findResultParametersByPacienteId(int id);

        CantidadPacientesPorSexo retornarPacientesPorSexo();

        CantidadPacientePorEdad retornarPacientesPorEdad();

        List<ResponsePacientesObesidad> retornarCantidadPacientesPorEdadObesidad(string sexo);

    }
}