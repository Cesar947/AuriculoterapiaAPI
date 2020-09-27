using System;

namespace Auriculoterapia.Api.Helpers
{
    public class PacienteMaximaSesion
    {
        public int Id {get; set;}
        public int MaximaSesion {get; set;} 
        public DateTime FechaNacimiento {get; set;}
    }
}