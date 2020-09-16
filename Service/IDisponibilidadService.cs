using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using System.Collections.Generic;

namespace Auriculoterapia.Api.Service
{
    public interface IDisponibilidadService: IService<Disponibilidad>
    {
        bool registrarDisponibilidad(FormularioDisponibilidad entity, int especialistaId);

        Disponibilidad listarPorFecha(string fecha);

        AvailabilityTimeRange listarHorasDisponibles(string fecha);
    }
}