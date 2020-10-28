using System;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Auriculoterapia.Api.Repository.Implementation
{
    public class CitaRepository: ICitaRepository
    {
        private ApplicationDbContext context;

        public CitaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Cita> FindAll(){
            var citas = new List<Cita>();
            try{
                citas = this.context.Citas.Include(c => c.Paciente)
                .Include(c => c.Paciente.Usuario)
                .Include(c => c.TipoAtencion)
                .OrderByDescending(c => c.Estado).ThenByDescending(c => c.Id).ToList();
            } catch(System.Exception){
                throw;
            }
            return citas;
        }
        public void Save(Cita entity){
            try{
                this.context.Citas.Add(entity);
                this.context.SaveChanges();
            }catch(System.Exception){
                throw;
            }
        }

        public Cita FindById(int id){
            var cita = new Cita();
            try{
                cita = this.context.Citas.Include(c => c.Paciente).Include(u => u.Paciente.Usuario)
                        .FirstOrDefault(c => c.Id == id);

            } catch(System.Exception){
                throw;
            }
            return cita;
        }

        public IEnumerable<Cita> listarCitasPorUsuarioId(int usuarioId){
            var citas = new List<Cita>();
            var estados = new List<String>{"En Proceso", "Completado", "Cancelado"};
            try{
                citas = this.context.Citas.Include(c => c.Paciente)
                .Include(c => c.Paciente.Usuario)                
                .Where(c => c.Paciente.Usuario.Id == usuarioId)
                .OrderByDescending(c => c.Estado).ThenByDescending(c => c.Id)
                .ToList();

                
            }catch(System.Exception){
                throw;
            }
            return citas;
        }


        public bool actualizarEstadoCita(int citaId, string estado){
            var cita = new Cita();
            var actualizado = false;
            try{
                cita = this.context.Citas.FirstOrDefault(c => c.Id == citaId);
                cita.Estado = estado;
                this.context.SaveChanges();
                actualizado = true;

            }catch(System.Exception){
                throw;
            }
            return actualizado;

        }


        public bool actualizarCita(Cita entity, int id){
            var citaActualizada = false;
            var citaAntigua = new Cita();
            try{
                citaAntigua = this.context.Citas.FirstOrDefault(c => c.Id == id);

                citaAntigua = entity;
                this.context.SaveChanges();
                citaActualizada = true;
            } catch(System.Exception){
                throw;
            }
            return citaActualizada;
        }

       
    }
}
