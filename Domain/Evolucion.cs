
namespace Auriculoterapia.Api.Domain
{
    public class Evolucion
    {
        public int Id {get; set;}
        public int EvolucionNumero {get;set;}
        public float Peso {get; set;}
        public string Otros {get;set;}
        public int Sesion {get;set;}
        public string TipoTratamiento {get;set;}
        public int TratamientoId {get;set;}
        public Tratamiento Tratamiento {get;set;}

    }
}