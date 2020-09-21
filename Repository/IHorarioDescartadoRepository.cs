using System;
using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Repository
{
    public interface IHorarioDescartadoRepository: IRepository<HorarioDescartado>
    {
        void actualizarHorarioDescartado(DateTime horaInicio, DateTime horaFin, 
        DateTime nuevaHoraInicio, DateTime nuevaHoraFin, Disponibilidad dispAnt, Disponibilidad dispAct);
        
        bool borrarPorDisponibilidadHoraInicio(DateTime horaInicio, Disponibilidad disp);
    }
}