using System;

namespace Auriculoterapia.Api.Domain
{
    public class Tratamiento
    {
        public int Id {get; set;}
        public string TipoTratamiento {get; set;}
        public DateTime FechaEnvio {get; set;}
        public DateTime FechaInicio {get;set;}
        public DateTime FechaFin {get; set;}
        public string Estado {get; set;}
        public int FrecuenciaAlDia {get; set;}
        public int TiempoPorTerapia {get; set;}
        public string ImagenEditada{get; set;}
        public int SolicitudTratamientoId {get; set;}
        public SolicitudTratamiento SolicitudTratamiento {get; set;}
    }
}