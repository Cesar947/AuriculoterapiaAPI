using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Auriculoterapia.Api.Helpers;
namespace Auriculoterapia.Api.Repository.Implementation
{
    public class DisponibilidadRepository : IDisponibilidadRepository
    {

        private ApplicationDbContext context;

        public DisponibilidadRepository(ApplicationDbContext context){
            this.context = context;
        }

        public IEnumerable<Disponibilidad> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Disponibilidad FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(Disponibilidad entity)
        {
            try{
                this.context.Disponibilidades.Add(entity);
                this.context.SaveChanges();
            }catch(System.Exception){
                throw;
            }
        }

        public  Disponibilidad guardarDisponibilidad(Disponibilidad entity){
                var disponibilidad = new Disponibilidad();
                
                try{    
    
                    this.context.Disponibilidades.Add(entity);
                    this.context.SaveChanges();
                    disponibilidad = entity;
                } catch(System.Exception){
                    throw;
                }
                return disponibilidad;
         }

         public Disponibilidad listarPorFecha(string fecha){
             var disponibilidad = new Disponibilidad();
             var conversor = new ConversorDeFechaYHora();
             try{
                disponibilidad = this.context.Disponibilidades.Include(d => d.HorariosDescartados)
                                    .FirstOrDefault(d => d.Dia == conversor.TransformarAFecha(fecha));
             } catch(System.Exception){
                    throw;
             }
             return disponibilidad;
         }

         public bool actualizarDisponibilidad(Disponibilidad entity){
            bool actualizo = false;
            var nDisponibilidad = new Disponibilidad();

            try{
                nDisponibilidad = this.context.Disponibilidades.FirstOrDefault(d => d.Dia == entity.Dia);
                nDisponibilidad.HoraInicio = entity.HoraInicio;
                nDisponibilidad.HoraFin = entity.HoraFin;
                nDisponibilidad.EspecialistaId = entity.EspecialistaId;
                nDisponibilidad.Especialista = entity.Especialista;
                nDisponibilidad.HorariosDescartados = entity.HorariosDescartados;
                this.context.SaveChanges();
            } catch(System.Exception){
                throw;


            }
            return true;
         }


        public AvailabilityTimeRange obtenerListaHorasDisponibles(string fecha){
            var listarHorasDisponibles = new AvailabilityTimeRange();
            var disponibilidad = new Disponibilidad();
            var conversor = new ConversorDeFechaYHora();
            
            try{
                var fechaConvertida = conversor.TransformarAFecha(fecha);
                disponibilidad = listarPorFecha(fecha);
                if(disponibilidad!= null){
                    listarHorasDisponibles.setList(disponibilidad.HoraInicio, disponibilidad.HoraFin,
                    disponibilidad.HorariosDescartados.ToList());

                }
                else {
                    listarHorasDisponibles.setList(new DateTime(fechaConvertida.Year,
                    fechaConvertida.Month, fechaConvertida.Day, 7, 0, 0),
                    new DateTime(fechaConvertida.Year,
                    fechaConvertida.Month, fechaConvertida.Day, 19, 0, 0),
                    null);

                }
                if (listarHorasDisponibles.hours.Count == 0){
                    return null;
                }
            }catch(System.Exception){
                throw;
            }
            return listarHorasDisponibles;
        }   

    }
}