using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Repository
{
    public interface ISolicitudTratamientoRepository: IRepository<SolicitudTratamiento>
    {
        SolicitudTratamiento findByPacienteId(int pacienteId);

        void saveByUserId(SolicitudTratamiento entity,int userId);

        string obtenerImagenPorSolicitud(int solicitudId);

        
        bool actualizarEstadoDeSolicitudDeTratamiento(int solicitudId, string estado);

        int contarSolicitudesEnProcesoDelPaciente(int pacienteId);
    
    }   


}