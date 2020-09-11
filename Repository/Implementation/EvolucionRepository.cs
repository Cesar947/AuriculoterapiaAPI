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
                    .FirstOrDefault(x => solicitudTratamiento.PacienteId == IdPaciente &&
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