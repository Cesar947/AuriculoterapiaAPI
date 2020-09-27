using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AuriculoterapiaAPI.Helpers;
using System;

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

        public IEnumerable<ResponseResultsPatient> getByIdPaciente_TipoTratamiento_Results(string TipoTratamiento, int idPaciente){
            var listaResponseResultsPatient = new List<ResponseResultsPatient>();
            var listaEvolucion = new List<Evolucion>();
            try{
                listaEvolucion = context.Evoluciones
                    .Include(x => x.Tratamiento)
                    .Include(x => x.Tratamiento.SolicitudTratamiento)
                    .Include(x => x.Tratamiento.SolicitudTratamiento.Paciente)
                    .Include(x => x.Tratamiento.SolicitudTratamiento.Paciente.Usuario)
                    .Where(x => x.TipoTratamiento == TipoTratamiento &&
                    x.Tratamiento.SolicitudTratamiento.PacienteId == idPaciente)
                    .ToList();

                foreach(var lista in listaEvolucion){
                    float altura = lista.Tratamiento.SolicitudTratamiento.Altura;
                    double IMC = lista.Peso/(altura*altura);
                    var sexo = lista.Tratamiento.SolicitudTratamiento.Paciente.Usuario.Sexo;


                    DateTime birth = DateTime.Parse(lista.Tratamiento.SolicitudTratamiento.Paciente.FechaNacimiento.ToString());
                    DateTime today = DateTime.Today;
                    int edad = today.Year - birth.Year;
                    //int age = int.Parse(edad.ToString());

                    if (today.Month < birth.Month ||
                    ((today.Month == birth.Month) && (today.Day < birth.Day)))
                    {
                        edad--;
                    }
                    double grasaCorporal;
                    if(sexo=="Masculino"){
                        grasaCorporal = 1.2*IMC+(0.23*edad)-(10.8*1)-5.4;
                    }else{
                        grasaCorporal = 1.2*IMC+(0.23*edad)-(10.8*0)-5.4;
                    }

                    var newResponse = new ResponseResultsPatient(lista.EvolucionNumero,lista.Peso,lista.Sesion,
                    lista.TipoTratamiento,lista.TratamientoId,Math.Round(IMC,1),Math.Round(grasaCorporal,1));

                    listaResponseResultsPatient.Add(newResponse);
                }
            }catch{
                throw;
            }
            return listaResponseResultsPatient;
            
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
                    
                    entity.Sesion = evolucionAnteriores.Sesion + 1;
                    
                }else{
                    entity.Sesion = 1;
                }
                
                tratamiento.Estado = "Completado";
                context.Evoluciones.Add(entity);
                context.SaveChanges();
            }catch{
                throw;
            }
        }


        
    }
}