using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository;
using System.Collections.Generic;
using Auriculoterapia.Api.Helpers;
using System;
using MailKit.Net.Smtp;
using MimeKit;

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

        private INotificacionRepository notificacionRepository;

    

        public CitaService(ICitaRepository CitaRepository, IPacienteRepository PacienteRepository, ITipoAtencionRepository tipoAtencionRepository,
        IDisponibilidadRepository disponibilidadRepository, IHorarioDescartadoRepository horarioDescartadoRepository,
        IEspecialistaRepository especialistaRepository,INotificacionRepository notificacionRepository){
            this.CitaRepository = CitaRepository;
            this.PacienteRepository = PacienteRepository;
            this.tipoAtencionRepository = tipoAtencionRepository;
            this.disponibilidadRepository = disponibilidadRepository;
            this.horarioDescartadoRepository = horarioDescartadoRepository;
            this.especialistaRepository = especialistaRepository;
            this.notificacionRepository = notificacionRepository;
        }

        public void Save(Cita entity){
            CitaRepository.Save(entity);
        }

        public void RegistrarCita(FormularioCita entity, int PacienteId){
            var cita = new Cita();
            var conversor = new ConversorDeFechaYHora(); 
            var correo = new SendEmail();
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

                    var notificacion = new Notificacion();
                    notificacion.EmisorId = 1;
                    notificacion.ReceptorId = paciente.UsuarioId;
                    notificacion.TipoNotificacion = "NUEVACITA";

                    notificacionRepository.Save(notificacion);

                    string emailUserTo = cita.Paciente.Usuario.Email;
                    string subject = "Nueva cita";
                    string nombrePaciente = cita.Paciente.Usuario.Nombre + " "+ cita.Paciente.Usuario.Apellido;

                    string horaCitaEmail = cita.HoraInicioAtencion.ToString();
                    //string nombreEspecialistaEmail = disponibilidad.Especialista.Usuario.Nombre + " "+disponibilidad.Especialista.Usuario.Apellido;
                    string textBody = "El especialista Samuel Chung registró una nueva cita para la siguiente fecha: " + horaCitaEmail;

                    correo.sendEmailTo(nombrePaciente,emailUserTo,subject,textBody);

                                       

                }catch(System.Exception){

                    throw;
                }
        }

        public void RegistrarCitaPaciente(FormularioCitaPaciente entity, int PacienteId){
            var cita = new Cita();
            var conversor = new ConversorDeFechaYHora(); 
            var disponibilidad = new Disponibilidad();
            var correo = new SendEmail();

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
                    disponibilidad.HoraFin = conversor.TransformarAHora("18:00", entity.Fecha);
                    disponibilidad.EspecialistaId = 1;
                    disponibilidad.Especialista = this.especialistaRepository.FindById(disponibilidad.EspecialistaId);
                    var disInserted = this.disponibilidadRepository.guardarDisponibilidad(disponibilidad); 

                    horarioDescartado.DisponibilidadId = disInserted.Id;
                    horarioDescartado.Disponibilidad = disInserted;
                }

                this.horarioDescartadoRepository.Save(horarioDescartado);

                var notificacion = new Notificacion();
                notificacion.EmisorId =paciente.UsuarioId;
                notificacion.ReceptorId = 1;
                notificacion.TipoNotificacion ="NUEVACITA";

                notificacionRepository.Save(notificacion);

                var especialista = especialistaRepository.FindById(1);

                string emailUserTo = especialista.Usuario.Email;
                string nombreEspecialista = especialista.Usuario.Nombre + " "+ especialista.Usuario.Apellido;

                string nombrePaciente = cita.Paciente.Usuario.Nombre + " "+ cita.Paciente.Usuario.Apellido;

                string horaCitaEmail = cita.HoraInicioAtencion.ToString();
            
                string textBody = "El paciente "+ nombrePaciente +" registró una cita para la siguiente fecha: " + horaCitaEmail;
            
                string subject = "NUeva Cita";

                correo.sendEmailTo(nombreEspecialista,emailUserTo,subject,textBody);

            }catch(System.Exception){

                throw;
            }
        }

        public bool actualizarCita(int id, FormularioCitaPaciente form){
            var cita = new Cita();
            var conversor = new ConversorDeFechaYHora(); 
            var disponibilidadAnterior = new Disponibilidad();
            var disponibilidadActual = new Disponibilidad();
            var citaActualizada = false;
            var horaInicioAnterior = new DateTime();
            var horaFinAnterior = new DateTime();
            var horaInicioActual = new DateTime();
            var horaFinActual = new DateTime();
            var correo = new SendEmail();
            try{
                cita = this.CitaRepository.FindById(id);
                horaInicioAnterior = cita.HoraInicioAtencion;
                horaFinAnterior = cita.HoraFinAtencion;
                horaInicioActual = conversor.TransformarAHora(form.HoraInicioAtencion, form.Fecha);
                horaFinActual = conversor.TransformarAHora(form.HoraFinAtencion, form.Fecha);

                disponibilidadAnterior = disponibilidadRepository.listarPorFecha(cita.Fecha.ToString("yyyy-MM-dd"));

                disponibilidadActual = disponibilidadRepository.listarPorFecha(form.Fecha);

                var horarioDescartado = new HorarioDescartado();
                horarioDescartado.HoraInicio = horaInicioActual;
                horarioDescartado.HoraFin = horaFinActual;

                
                if (disponibilidadAnterior != null){
                    if(disponibilidadActual == null){

                        disponibilidadActual = new Disponibilidad();
                        disponibilidadActual.Dia = conversor.TransformarAFecha(form.Fecha);
                        disponibilidadActual.HoraInicio = conversor.TransformarAHora("07:00", form.Fecha);
                        disponibilidadActual.HoraFin = conversor.TransformarAHora("18:00", form.Fecha);
                        disponibilidadActual.EspecialistaId = 1;
                        disponibilidadActual.Especialista = this.especialistaRepository.FindById(disponibilidadActual.EspecialistaId);
                        var disInserted = this.disponibilidadRepository.guardarDisponibilidad(disponibilidadActual); 

                        horarioDescartado.DisponibilidadId = disInserted.Id;
                        horarioDescartado.Disponibilidad = disInserted;
                        
                        this.horarioDescartadoRepository.Save(horarioDescartado);

                    }
              
               
                 this.horarioDescartadoRepository.actualizarHorarioDescartado(horaInicioAnterior, horaFinAnterior,
                             horaInicioActual, horaFinActual, disponibilidadAnterior, disponibilidadActual);
                var tipoAtencion = tipoAtencionRepository.FindByDescription(form.TipoAtencion);
                PacienteRepository.ActualizarNumeroPaciente(form.Celular, cita.Paciente);
                cita.Fecha = conversor.TransformarAFecha(form.Fecha);
                cita.HoraInicioAtencion = conversor.TransformarAHora(form.HoraInicioAtencion, form.Fecha);
                cita.HoraFinAtencion = conversor.TransformarAHora(form.HoraFinAtencion, form.Fecha);
                cita.TipoAtencionId = tipoAtencion.Id;
                cita.TipoAtencion = tipoAtencion;
                 

                citaActualizada = this.CitaRepository.actualizarCita(cita, id);   
                }

                var notificacion = new Notificacion();
                notificacion.EmisorId = cita.Paciente.UsuarioId;
                notificacion.ReceptorId = 1;
                notificacion.TipoNotificacion = "MODIFICARCITA";

                notificacionRepository.Save(notificacion);

                var especialista = especialistaRepository.FindById(1);

                string emailUserTo = especialista.Usuario.Email;
                string nombreEspecialista = especialista.Usuario.Nombre + " "+ especialista.Usuario.Apellido;

                string nombrePaciente = cita.Paciente.Usuario.Nombre + " "+ cita.Paciente.Usuario.Apellido;

                string horaCitaEmail = cita.HoraInicioAtencion.ToString();
            
                string textBody = "El paciente "+ nombrePaciente +" modificó la cita para la siguiente fecha: " + horaCitaEmail;
            
                string subject = "Cita Modificada";

                correo.sendEmailTo(nombreEspecialista,emailUserTo,subject,textBody);
                

            } catch(System.Exception){
                throw;
            }
            return citaActualizada;
        }

      


        public void procesarHorariosDescartados(Cita cita, FormularioCita fs=null, FormularioCitaPaciente fp=null){


        }

        public Cita findByIdParaPaciente(int id){
            return CitaRepository.FindById(id);
        }
        public IEnumerable<Cita> FindAll(){
           return CitaRepository.FindAll();
        }
    
        public IEnumerable<Cita> listarCitasPorUsuarioId(int usuarioId){
            return CitaRepository.listarCitasPorUsuarioId(usuarioId);
        }
        public bool actualizarEstadoCita(int citaId, string estado, int usuarioId){
            var disponibilidad = new Disponibilidad();
            var horarioBorrado = false;
            var cita = new Cita();
            var correo = new SendEmail();
            try{
                if(estado.Equals("Cancelado")){
                    cita = this.CitaRepository.FindById(citaId);
                    disponibilidad = this.disponibilidadRepository.listarPorFecha(cita.Fecha.ToString("yyyy-MM-dd"));
                    if(disponibilidad != null){
                    horarioBorrado = this.horarioDescartadoRepository.borrarPorDisponibilidadHoraInicio(cita.HoraInicioAtencion, disponibilidad);
                    }

                    var notificacion = new Notificacion();

                    string subject = "Cita Cancelada";

                    if(usuarioId == 1){
                        notificacion.EmisorId = usuarioId;
                        notificacion.ReceptorId = cita.Paciente.UsuarioId;

                        //EMAIL
                        string emailUserTo = cita.Paciente.Usuario.Email;
                  
                        string nombrePaciente = cita.Paciente.Usuario.Nombre + " "+ cita.Paciente.Usuario.Apellido;

                        string horaCitaEmail = cita.HoraInicioAtencion.ToString();
                    
                        string textBody = "El especialista Samuel Chung canceló la cita para la siguiente fecha: " + horaCitaEmail;

                        correo.sendEmailTo(nombrePaciente,emailUserTo,subject,textBody);

                    }else{
                        var especialista = especialistaRepository.FindById(1);
                        notificacion.EmisorId = usuarioId;
                        notificacion.ReceptorId = 1;

                        string emailUserTo = especialista.Usuario.Email;
                        string nombreEspecialista = especialista.Usuario.Nombre + " "+ especialista.Usuario.Apellido;

                        string nombrePaciente = cita.Paciente.Usuario.Nombre + " "+ cita.Paciente.Usuario.Apellido;

                        string horaCitaEmail = cita.HoraInicioAtencion.ToString();
                    
                        string textBody = "El paciente "+ nombrePaciente +" canceló la cita para la siguiente fecha: " + horaCitaEmail;
                    
                        correo.sendEmailTo(nombreEspecialista,emailUserTo,subject,textBody);

                    
                    }
                    notificacion.TipoNotificacion = "CANCELARCITA";
                    notificacionRepository.Save(notificacion);

                   

                    

                }
              
            

            } catch(System.Exception){

            }


            return CitaRepository.actualizarEstadoCita(citaId, estado);
        }
    }
}