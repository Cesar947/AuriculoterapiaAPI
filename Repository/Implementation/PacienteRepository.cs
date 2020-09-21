using System;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Auriculoterapia.Api.Helpers;

namespace Auriculoterapia.Api.Repository.Implementation
{
    public class PacienteRepository: IPacienteRepository
    {
        private ApplicationDbContext context;

        private readonly IUsuarioRepository usuarioRepository;


        public PacienteRepository(ApplicationDbContext context,IUsuarioRepository usuarioRepository)
        {
            this.context = context;
            this.usuarioRepository = usuarioRepository;
        }

        public IEnumerable<Paciente> FindAll(){
            var pacientes = new List<Paciente>();
            try{
                pacientes = this.context.Pacientes.Include(p => p.Usuario).ToList();
            }catch(System.Exception){
                    throw;
            }
            return pacientes;
        }
        public void Save(Paciente entity){
            try{

                var isValidEmail = usuarioRepository.IsValidEmail(entity.Usuario.Email); 
                
                if(isValidEmail){
                    usuarioRepository.Save(entity.Usuario);
                    if(entity.Usuario.Id > 0){
                        context.Add(entity);
                        context.SaveChanges();
                    }
                }
                else{
                    entity.Id = -1;
                    Console.WriteLine("Correo Inválido");
                }
                
            }catch{
                throw;
            }
            
        }

        
        public Paciente FindById(int Id){
            var paciente = new Paciente();
            try{
                paciente = this.context.Pacientes.Single(paciente => paciente.Id == Id);
            }catch(System.Exception){
                throw;
            }
            return paciente;
        }


        public Paciente buscarPorUsuarioId(int usuarioId){
                var paciente = new Paciente();
            try{
                paciente = this.context.Pacientes.Single(paciente => paciente.UsuarioId == usuarioId);
            }catch(System.Exception){
                throw;
            }
            return paciente;
        }



        public string ActualizarNumeroPaciente(string numero, Paciente paciente){
            string actualizado = "Actualizado";
            try{
                paciente.Celular = numero;
                this.context.SaveChanges();
            } catch{

            }
            return actualizado;
        }

        public IEnumerable<Paciente> busquedaPacientePorPalabra(string palabras){
           var pacientes = new List<Paciente>();
            try{
                /*pacientes = (from p in this.context.Pacientes
                            where (terminos.Any(r  => p.Usuario.Nombre.Contains(r))) ||
                                  (terminos.Any(r  => p.Usuario.Apellido.Contains(r)))
                            select p).ToList();*/
                var dbListPacientes = from s in this.context.SolicitudTratamientos
                                        join p in this.context.Pacientes on s.PacienteId equals p.Id
                                        select p;
                                        
                if (!String.IsNullOrEmpty(palabras)){
                    dbListPacientes = dbListPacientes.Distinct().Where(p => (p.Usuario.Nombre + " " + p.Usuario.Apellido).Contains(palabras));
                }
                pacientes = dbListPacientes.Distinct().Include(p => p.Usuario).ToList();
                
            } catch(System.Exception){
                throw;
            }
            return pacientes;

        }

        public CantidadPacientesPorSexo retornarPacientesPorSexo(string tratamiento){
            var estadistica = new CantidadPacientesPorSexo();
     
            try{
               /*var dbQueryHombres = from u in context.Usuarios
                            join p in context.Pacientes on u.Id equals p.UsuarioId
                            join s in context.SolicitudTratamientos on p.Id equals s.PacienteId
                            join t in context.Tratamientos on s.Id equals t.SolicitudTratamientoId
                            where t.TipoTratamiento == tratamiento 
                            where u.Sexo == "Masculino"
                            select new {
                                p.Id
                            };*/
                 var dbQueryHombres = this.context.Tratamientos
                .Include(t => t.SolicitudTratamiento)
                .Where(t => t.TipoTratamiento == tratamiento)
                .Where(t => t.SolicitudTratamiento.Paciente.Usuario.Sexo == "Masculino")
                .Select(t => new { pId = t.SolicitudTratamiento.PacienteId})
                .ToList();

                /*var dbQueryMujeres = from u in context.Usuarios
                            join p in context.Pacientes on u.Id equals p.UsuarioId
                            join s in context.SolicitudTratamientos on p.Id equals s.PacienteId
                            join t in context.Tratamientos on s.Id equals t.SolicitudTratamientoId
                            where t.TipoTratamiento == tratamiento 
                            where u.Sexo == "Femenino"
                            select new {
                                p.Id
                            };*/

                var dbQueryMujeres = this.context.Tratamientos
                .Include(t => t.SolicitudTratamiento)
                .Where(t => t.TipoTratamiento == tratamiento)
                .Where(t => t.SolicitudTratamiento.Paciente.Usuario.Sexo == "Femenino")
                .Select(t => new { pId = t.SolicitudTratamiento.PacienteId})
                .ToList();
                
                
                estadistica.cantidadHombres = dbQueryHombres.Distinct().Count();
                estadistica.cantidadMujeres = dbQueryMujeres.Distinct().Count();


            } catch(System.Exception){
                throw;
            }

            return estadistica;


        }

    }
}
