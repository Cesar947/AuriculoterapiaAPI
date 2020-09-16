using System;
using System.Collections.Generic;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using System.Linq;
using Auriculoterapia.Api.Repository.Context;

namespace Auriculoterapia.Api.Repository.Implementation
{
    public class HorarioDescartadoRepository : IHorarioDescartadoRepository
    {

        private ApplicationDbContext context;

        public HorarioDescartadoRepository(ApplicationDbContext context){
            this.context = context;
        }


        public IEnumerable<HorarioDescartado> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public HorarioDescartado FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Save(HorarioDescartado entity)
        {
            try{
                this.context.HorariosDescartados.Add(entity);
                this.context.SaveChanges();
            }catch(System.Exception){
                throw;
            }
        }


        public void actualizarHorarioDescartado(DateTime horaInicio, DateTime horaFin, 
        DateTime nuevaHoraInicio, DateTime nuevaHoraFin, Disponibilidad dispAnt, Disponibilidad dispAct){

            var horarioDescartadoAntiguo = new HorarioDescartado();
            var horarioDescartadoNuevo = new HorarioDescartado();
            var conversor = new ConversorDeFechaYHora();
            try{
    
               horarioDescartadoAntiguo = this.context.HorariosDescartados
                .FirstOrDefault(h => h.Disponibilidad.Id == dispAnt.Id && h.HoraInicio == horaInicio 
                && h.HoraFin == horaFin);

               horarioDescartadoNuevo = this.context.HorariosDescartados
                .FirstOrDefault(h => h.Disponibilidad.Id == dispAct.Id && h.HoraInicio == nuevaHoraInicio
                && h.HoraFin == nuevaHoraFin);
                
                if(horarioDescartadoNuevo != null){
                    if(horarioDescartadoAntiguo.Equals(horarioDescartadoNuevo)){
                        horarioDescartadoAntiguo.HoraInicio = nuevaHoraInicio;
                        horarioDescartadoAntiguo.HoraFin = nuevaHoraFin;
                        this.context.SaveChanges();
                    }
                } else{
                    this.context.HorariosDescartados.Remove(horarioDescartadoAntiguo);
                    
                    horarioDescartadoNuevo = new HorarioDescartado();
                    horarioDescartadoNuevo.HoraInicio = nuevaHoraInicio;
                    horarioDescartadoNuevo.HoraFin = nuevaHoraFin;
                    horarioDescartadoNuevo.Disponibilidad = dispAct;
                    horarioDescartadoNuevo.Disponibilidad.Id = dispAct.Id;
                    
                    this.context.HorariosDescartados.Add(horarioDescartadoNuevo);
                    this.context.SaveChanges();
                    
                }
                

            } catch(System.Exception){
                    throw;
            }


        }
        
        public bool borrarPorDisponibilidadHoraInicio(DateTime horaInicio, Disponibilidad disp){
            var horario = new HorarioDescartado();
            var borrado = false;
            try{
                horario = this.context.HorariosDescartados
                .FirstOrDefault(h => h.HoraInicio == horaInicio && h.Disponibilidad == disp);

                this.context.HorariosDescartados.Remove(horario);
                this.context.SaveChanges();

                borrado = true;

            } catch(System.Exception){
                throw;
            }
            return borrado;
         
        }   



    }
}