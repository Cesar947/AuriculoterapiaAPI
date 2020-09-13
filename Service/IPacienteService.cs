using System;
using Auriculoterapia.Api.Domain;
using System.Collections.Generic;
using Auriculoterapia.Api.Helpers;


namespace Auriculoterapia.Api.Service
{
    public interface IPacienteService: IService<Paciente>
    {
        IEnumerable<Paciente> buscarPorPalabra(string palabra);

        CantidadPacientesPorSexo retornarPacientesPorSexo(string tratamiento);
    }
}