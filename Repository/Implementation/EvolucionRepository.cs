using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Auriculoterapia.Api.Repository.Implementation
{
    public class EvolucionRepository : IEvolucionRepository
    {
        private ApplicationDbContext context;

        public EvolucionRepository(ApplicationDbContext context){
            this.context = context;
        }

        public IEnumerable<Evolucion> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Evolucion FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Evolucion> getByIdPaciente_TipoTratamiento(string TipoTratamiento,int idPaciente){
            var listaEvolucion = new List<Evolucion>();
            try{
                listaEvolucion = context.Evoluciones
                    //.Include(x => x.Tratamiento)
                    //.Include(x => x.Tratamiento.SolicitudTratamiento)
                    .Where(x => x.TipoTratamiento == TipoTratamiento &&
                    x.Tratamiento.SolicitudTratamiento.PacienteId == idPaciente)
                    .ToList();

            }catch{
                throw;
            }
            return listaEvolucion;

        }

        public void Save(Evolucion entity)
        {
            try{
                var tratamiento = context.Tratamientos.FirstOrDefault(x =>x.Id == entity.TratamientoId);


                context.Evoluciones.Add(entity);
                context.SaveChanges();
            }catch{
                throw;
            }
        }

        public void saveByIdPaciente(Evolucion entity,int IdPaciente){
             try{
                var tratamiento = context.Tratamientos.FirstOrDefault(x =>x.Id == entity.TratamientoId); //id unico
                
                var solicitudTratamiento = context.SolicitudTratamientos.FirstOrDefault(x => x.Id == tratamiento.SolicitudTratamientoId); //id unico
                //var evolucionAnteriores = context.Evoluciones.LastOrDefault(x => x.Tratamiento.TipoTratamiento == entity.TratamientoId);
                var evolucionAnteriores = context.Evoluciones
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault(x => x.Tratamiento.SolicitudTratamiento.PacienteId == IdPaciente &&
                    x.TipoTratamiento == entity.TipoTratamiento);
                    
                if(evolucionAnteriores!= null){
                    
                    entity.Sesion = evolucionAnteriores.Sesion +1;
                    
                }else{
                    entity.Sesion = 1;
                }
                
                context.Evoluciones.Add(entity);
                context.SaveChanges();
            }catch{
                throw;
            }
        }
    }
}