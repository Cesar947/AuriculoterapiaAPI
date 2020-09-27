namespace Auriculoterapia.Api.Helpers
{
    public class ResponsePacientesObesidad
    {
        public string TipoPacientePorEdad {get;set;}
        public int Cantidad {get; set;}
        public double ImcPromedio {get; set;}
        public double PorcentajeGcPromedio {get; set;}
        public string TipoIndicadorImc {get; set;}
        public string TipoIndicadorGc {get; set;}
    }
}