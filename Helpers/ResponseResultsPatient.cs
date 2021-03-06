namespace AuriculoterapiaAPI.Helpers
{
    public class ResponseResultsPatient
    {
        public int EvolucionNumero {get;set;}
        public float Peso {get; set;}
        public int Sesion {get;set;}
        public string TipoTratamiento {get;set;}
        public int TratamientoId {get;set;}

        public double Imc{get;set;}

        public double grasaCorporal{get;set;}

        public string Otros {get;set;}

        public ResponseResultsPatient(int EvolucionNumero,float Peso, int Sesion,string TipoTratamiento,int TratamientoId, double Imc, double grasaCorporal, string Otros ){
            this.EvolucionNumero = EvolucionNumero;
            this.Peso = Peso;
            this.Sesion = Sesion;
            this.TipoTratamiento = TipoTratamiento;
            this.TratamientoId = TratamientoId;
            this.Imc = Imc;
            this.grasaCorporal = grasaCorporal;
            this.Otros = Otros;
        }

    }
}