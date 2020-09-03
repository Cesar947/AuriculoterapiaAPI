using System;
using System.Collections.Generic;
using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Helpers
{
    public class AvailabilityTimeRange
    {
        public List<string> hours {get; set;}

        public AvailabilityTimeRange(){
            this.hours = new List<string>();
        }
        public void setList(DateTime minHour, DateTime maxHour, List<HorarioDescartado> horariosDescartados){
            var conversor = new ConversorDeFechaYHora();
            DateTime currentHour = minHour;
            var todayHour = DateTime.Now;
            var realMaxHour =  maxHour - new TimeSpan(0, 30, 0);
            if (todayHour > currentHour && todayHour <= realMaxHour)
            {
                currentHour = todayHour;

                if(currentHour.Minute <= 30){
                    currentHour += new TimeSpan(0, 30 - currentHour.Minute, 0);
                }
                else if(currentHour.Minute > 30 ){
                    currentHour += new TimeSpan(0, 60 - currentHour.Minute , 0);
                }

            } else if(todayHour > realMaxHour){
                return;
            }


            int c;
            while(currentHour <= realMaxHour){
                c = 0;
                if(horariosDescartados != null){
                    foreach(var h in horariosDescartados){
                    if(currentHour >= h.HoraInicio && currentHour < h.HoraFin){
                        c = 1;
                    }
                }
                }
                
                if (c == 1){
                    currentHour += new TimeSpan(0, 30, 0);
                    continue;
                } else {
                    var itemHora = conversor.TransformarHoraATexto(currentHour);
                    hours.Add(itemHora);
                    currentHour += new TimeSpan(0, 30, 0);
                }
                
            }
        }
    }
}