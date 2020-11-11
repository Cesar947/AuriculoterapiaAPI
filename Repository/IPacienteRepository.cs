using System;
using Auriculoterapia.Api.Domain;
using System.Collections.Generic;
using Auriculoterapia.Api.Helpers;

namespace Auriculoterapia.Api.Repository
{
    public interface IPacienteRepository: IRepository<Paciente>
    {
       string ActualizarNumeroPaciente(string numero, Paciente paciente);

       IEnumerable<Paciente> busquedaPacientePorPalabra(string palabras);

       Paciente buscarPorUsuarioId(int usuarioId);

       List<Paciente> pacientesPorEdad(int min, int max, string tratamiento="", string sexo="");

       PacienteResultsParameters findResultParametersByPacienteId(int Id);

       CantidadPacientesPorSexo retornarPacientesPorSexo();

       CantidadPacientePorEdad retornarPacientesPorEdad();

       CantidadPacientesPorNivel retornarPacientesPorNivel(string tratamiento);

       ResponsePacientesObesidad retornarCantidadPacientesPorEdadObesidad(int min, int max, string sexo, string tipoPacientePorEdad);

    }
}
