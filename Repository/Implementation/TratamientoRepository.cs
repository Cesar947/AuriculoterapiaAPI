
using Auriculoterapia.Api.Repository.Context;
using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Auriculoterapia.Api.Repository.Implementation
{
    public class TratamientoRepository : ITratamientoRepository
    {

        private ApplicationDbContext context;

        public TratamientoRepository(ApplicationDbContext context){
            this.context = context;
        }

        public IEnumerable<Tratamiento> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Tratamiento FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(Tratamiento entity)
        {
           try{
               this.context.Tratamientos.Add(entity);
               this.context.SaveChanges();
           } catch(System.Exception){
                throw;
           }

        }

        public IEnumerable<Tratamiento> listarPorPacienteId(int pacienteId)
        {
            var tratamientosDePaciente = new List<Tratamiento>();
            try
            {
                tratamientosDePaciente = this.context.Tratamientos
                    .Include(t => t.SolicitudTratamiento)
                    .Include(t => t.SolicitudTratamiento.Paciente)
                    .Include(t => t.SolicitudTratamiento.Paciente.Usuario)
                    .Where(t => t.SolicitudTratamiento.Paciente.Id == pacienteId)
                    .ToList();
            }
            catch (System.Exception)
            {
                throw;
            }
            return tratamientosDePaciente;
        }
    }
}