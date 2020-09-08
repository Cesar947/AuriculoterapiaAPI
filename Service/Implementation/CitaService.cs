using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository;
using System.Collections.Generic;
using Auriculoterapia.Api.Helpers;


namespace Auriculoterapia.Api.Service.Implementation
{
    public class CitaService: ICitaService
    {
        private readonly ICitaRepository CitaRepository;
        private readonly IPacienteRepository PacienteRepository;
        private readonly ITipoAtencionRepository tipoAtencionRepository;

        private readonly IEspecialistaRepository especialistaRepository;
        private readonly IDisponibilidadRepository disponibilidadRepository;
        private IHorarioDescartadoRepository horarioDescartadoRepository;
    

        public CitaService(ICitaRepository CitaRepository, IPacienteRepository PacienteRepository, ITipoAtencionRepository tipoAtencionRepository,
        IDisponibilidadRepository disponibilidadRepository, IHorarioDescartadoRepository horarioDescartadoRepository,
        IEspecialistaRepository especialistaRepository){
            this.CitaRepository = CitaRepository;
            this.PacienteRepository = PacienteRepository;
            this.tipoAtencionRepository = tipoAtencionRepository;
            this.disponibilidadRepository = disponibilidadRepository;
            this.horarioDescartadoRepository = horarioDescartadoRepository;
            this.especialistaRepository = especialistaRepository;
      
        }

        public void Save(Cita entity){
            CitaRepository.Save(entity);
        }

        public void RegistrarCita(FormularioCita entity, int PacienteId){
            var cita = new Cita();
            var conversor = new ConversorDeFechaYHora(); 
             var disponibilidad = new Disponibilidad();
                try {
                    var tipoAtencion = tipoAtencionRepository.FindByDescription(entity.TipoAtencion);
                    var paciente = PacienteRepository.FindById(PacienteId);
                    cita.Fecha = conversor.TransformarAFecha(entity.Fecha);
                    cita.HoraInicioAtencion = conversor.TransformarAHora(entity.HoraInicioAtencion, entity.Fecha);
                    cita.HoraFinAtencion = conversor.TransformarAHora(entity.HoraFinAtencion, entity.Fecha);
                    cita.Estado = "En Proceso";
                    cita.PacienteId = paciente.Id;
                    cita.Paciente = paciente;
                    cita.TipoAtencionId = tipoAtencion.Id;
                    cita.TipoAtencion = tipoAtencion;
                    Save(cita);
                
                    var horarioDescartado = new HorarioDescartado();
                        horarioDescartado.HoraInicio = cita.HoraInicioAtencion;
                        horarioDescartado.HoraFin = cita.HoraFinAtencion;
                        
                    disponibilidad = disponibilidadRepository.listarPorFecha(entity.Fecha);
                    
                    if(disponibilidad != null){
                        
                            horarioDescartado.DisponibilidadId = disponibilidad.Id;
                            horarioDescartado.Disponibilidad = disponibilidad;

                            
                    } else {
                        disponibilidad = new Disponibilidad();
                        disponibilidad.Dia = cita.Fecha;
                        disponibilidad.HoraInicio = conversor.TransformarAHora("07:00", entity.Fecha);
                        disponibilidad.HoraFin = conversor.TransformarAHora("19:00", entity.Fecha);
                        disponibilidad.EspecialistaId = 1;
                        disponibilidad.Especialista = this.especialistaRepository.FindById(disponibilidad.EspecialistaId);
                        var disInserted = this.disponibilidadRepository.guardarDisponibilidad(disponibilidad); 

                        horarioDescartado.DisponibilidadId = disInserted.Id;
                        horarioDescartado.Disponibilidad = disInserted;
                    }

                    this.horarioDescartadoRepository.Save(horarioDescartado);

                }catch(System.Exception){

                    throw;
                }
        }

        public void RegistrarCitaPaciente(FormularioCitaPaciente entity, int PacienteId){
            var cita = new Cita();
            var conversor = new ConversorDeFechaYHora(); 
            var disponibilidad = new Disponibilidad();
            try {

                var tipoAtencion = tipoAtencionRepository.FindByDescription(entity.TipoAtencion);
                var paciente = PacienteRepository.buscarPorUsuarioId(PacienteId);
                PacienteRepository.ActualizarNumeroPaciente(entity.Celular, paciente);
                cita.Fecha = conversor.TransformarAFecha(entity.Fecha);
                cita.HoraInicioAtencion = conversor.TransformarAHora(entity.HoraInicioAtencion, entity.Fecha);
                cita.HoraFinAtencion = conversor.TransformarAHora(entity.HoraFinAtencion, entity.Fecha);
                cita.Estado = "En Proceso";
                cita.PacienteId = paciente.Id;
                cita.Paciente = paciente;
                cita.TipoAtencionId = tipoAtencion.Id;
                cita.TipoAtencion = tipoAtencion;
                Save(cita);

                var horarioDescartado = new HorarioDescartado();
                horarioDescartado.HoraInicio = cita.HoraInicioAtencion;
                horarioDescartado.HoraFin = cita.HoraFinAtencion;

                disponibilidad = disponibilidadRepository.listarPorFecha(entity.Fecha);
                
                if(disponibilidad != null){
                      
                        horarioDescartado.DisponibilidadId = disponibilidad.Id;
                        horarioDescartado.Disponibilidad = disponibilidad;

                        
                } else {
                    disponibilidad = new Disponibilidad();
                    disponibilidad.Dia = cita.Fecha;
                    disponibilidad.HoraInicio = conversor.TransformarAHora("07:00", entity.Fecha);
                    disponibilidad.HoraFin = conversor.TransformarAHora("19:00", entity.Fecha);
                    disponibilidad.EspecialistaId = 1;
                    disponibilidad.Especialista = this.especialistaRepository.FindById(disponibilidad.EspecialistaId);
                    var disInserted = this.disponibilidadRepository.guardarDisponibilidad(disponibilidad); 

                    horarioDescartado.DisponibilidadId = disInserted.Id;
                    horarioDescartado.Disponibilidad = disInserted;
                }

                this.horarioDescartadoRepository.Save(horarioDescartado);
                

            }catch(System.Exception){

                throw;
            }
        }

        public void procesarHorariosDescartados(Cita cita, FormularioCita fs=null, FormularioCitaPaciente fp=null){


        }

        public IEnumerable<Cita> FindAll(){
           return CitaRepository.FindAll();
        }
    
        public IEnumerable<Cita> listarCitasPorUsuarioId(int usuarioId){
            return CitaRepository.listarCitasPorUsuarioId(usuarioId);
        }
        public bool actualizarEstadoCita(int citaId, string estado){
            return CitaRepository.actualizarEstadoCita(citaId, estado);
        }
    }
}