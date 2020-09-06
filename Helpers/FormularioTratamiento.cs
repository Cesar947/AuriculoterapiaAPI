namespace Auriculoterapia.Api.Helpers
{
    public class FormularioTratamiento
    {
        public string TipoTratamiento {get; set;}
        public string FechaEnvio {get; set;}
        public string FechaInicio {get;set;}
        public string FechaFin {get; set;}
        public int FrecuenciaAlDia {get; set;}
        public int TiempoPorTerapia {get; set;}
        public string ImagenEditada{get; set;}
        public int SolicitudTratamientoId {get; set;}

    }
}