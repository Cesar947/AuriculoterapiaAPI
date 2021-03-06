using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using Auriculoterapia.Api.Repository;

namespace Auriculoterapia.Api.Service.Implementation
{
    public class TratamientoService : ITratamientoService
    {

        private ITratamientoRepository tratamientoRepository;
        private ISolicitudTratamientoRepository solicitudTratamientoRepository;
        private INotificacionRepository notificacionRepository;


        public TratamientoService(ITratamientoRepository tratamientoRepository,
        ISolicitudTratamientoRepository solicitudTratamientoRepository,
        INotificacionRepository notificacionRepository){
            this.tratamientoRepository = tratamientoRepository;
            this.solicitudTratamientoRepository = solicitudTratamientoRepository;
            this.notificacionRepository = notificacionRepository;
        }

        public IEnumerable<Tratamiento> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public bool registrarTratamiento(FormularioTratamiento t)
        {
            var nuevoTratamiento = new Tratamiento();
            var conversor = new ConversorDeFechaYHora();
            var registroExitoso = false;
            try{    
                nuevoTratamiento.TipoTratamiento = t.TipoTratamiento;
                nuevoTratamiento.FechaEnvio = conversor.TransformarAFecha(t.FechaEnvio);
                nuevoTratamiento.FechaInicio = conversor.TransformarAFecha(t.FechaInicio);
                nuevoTratamiento.FechaFin = conversor.TransformarAFecha(t.FechaFin);
                nuevoTratamiento.FrecuenciaAlDia = t.FrecuenciaAlDia;
                nuevoTratamiento.TiempoPorTerapia = t.TiempoPorTerapia;
                nuevoTratamiento.ImagenEditada = t.ImagenEditada;
                nuevoTratamiento.SolicitudTratamientoId = t.SolicitudTratamientoId;
                var solicitudAResponder =  solicitudTratamientoRepository.FindById(t.SolicitudTratamientoId);
                nuevoTratamiento.SolicitudTratamiento = solicitudAResponder;

                solicitudTratamientoRepository.actualizarEstadoDeSolicitudDeTratamiento(
                    nuevoTratamiento.SolicitudTratamientoId, "Respondido"
                );

                nuevoTratamiento.Estado = "En Proceso";
                
                tratamientoRepository.Save(nuevoTratamiento);                       
                registroExitoso = true;
                if(registroExitoso){
                    var notificacion = new Notificacion();
                    notificacion.EmisorId = 1;
                    notificacion.ReceptorId = solicitudAResponder.Paciente.UsuarioId;
                    notificacion.TipoNotificacion = "RESPONDERTRATAMIENTO";
                    notificacionRepository.Save(notificacion);  
                }
                

            }catch(System.Exception){
                throw;
            }
            return registroExitoso;
        }

        public void Save(Tratamiento entity)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Tratamiento> listarPorPacienteId(int pacienteId)
        {
            return this.tratamientoRepository.listarPorPacienteId(pacienteId);
        }

        public Tratamiento FindById(int id){
            return this.tratamientoRepository.FindById(id);
        }


    }
}