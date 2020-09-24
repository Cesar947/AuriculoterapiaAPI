namespace Auriculoterapia.Api.Helpers
{
    public class ResponsePacientesObesidad
    {
        public string TipoPacientePorEdad {get;set;}
        public int Cantidad {get; set;}
        public float ImcPromedio {get; set;}
        public float PorcentajeGcPromedio {get; set;}
        public float TipoIndicadorImc {get; set;}
        public float TipoIndicadorGc {get; set;}
    }
}