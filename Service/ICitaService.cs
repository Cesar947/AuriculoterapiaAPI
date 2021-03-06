using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using System.Collections.Generic;

namespace Auriculoterapia.Api.Service
{
    public interface ICitaService: IService<Cita>
    {
        void RegistrarCita(FormularioCita entity, int PacienteId);
        void RegistrarCitaPaciente(FormularioCitaPaciente entity, int PacienteId);
        IEnumerable<Cita> listarCitasPorUsuarioId(int usuarioId);

        bool actualizarEstadoCita(int citaId, string estado, int usuarioId);

        void procesarHorariosDescartados(Cita cita, FormularioCita fs=null, FormularioCitaPaciente fp=null);
    
        Cita findByIdParaPaciente(int id);

        bool actualizarCita(int id, FormularioCitaPaciente form);

    }
}