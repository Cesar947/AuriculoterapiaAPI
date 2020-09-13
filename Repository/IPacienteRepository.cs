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

       CantidadPacientesPorSexo retornarPacientesPorSexo(string tratamiento);
    }
}
