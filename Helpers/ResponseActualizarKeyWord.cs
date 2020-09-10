namespace Auriculoterapia.Api.Helpers
{
    public class ResponseActualizarKeyWord
    {
        public int Id {get; set;}
        public string PalabraClave {get; set;}
        public string NuevaPalabraClave {get; set;}

        public ResponseActualizarKeyWord(int Id,string PalabraClave,string NuevaPalabraClave){
            this.Id = Id;
            this.PalabraClave = PalabraClave;
            this.NuevaPalabraClave = NuevaPalabraClave;
        }
    }
}