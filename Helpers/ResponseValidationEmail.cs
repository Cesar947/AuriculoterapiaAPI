using System;

namespace Auriculoterapia.Api.Helpers
{
    public class ResponseValidationEmail
    {
        public int Id {get; set;}

        public DateTime? FechaEnvioCorreoConfirmacion {get;set;}

        public DateTime? FechaConfirmacionCodigo {get;set;}

        public string Codigo {get; set;}

        public bool EmailExist {get;set;}

        public ResponseValidationEmail(int id,DateTime? FechaEnvioCorreoConfirmacion,
         DateTime? FechaConfirmacionCodigo, string codigo, bool EmailExist ){
             this.Id = id;
             this.FechaConfirmacionCodigo = FechaConfirmacionCodigo;
             this.FechaEnvioCorreoConfirmacion = FechaEnvioCorreoConfirmacion;
             this.Codigo = codigo;
             this.EmailExist = EmailExist;

         }
    }
}