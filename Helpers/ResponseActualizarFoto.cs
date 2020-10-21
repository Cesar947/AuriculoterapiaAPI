namespace Auriculoterapia.Api.Helpers
{
    public class ResponseActualizarFoto
    {
        public int Id {get; set;}

        public string Foto {get;set;}

        public ResponseActualizarFoto(int id, string foto){
            this.Id = id;
            this.Foto = foto;
        }
    }
}