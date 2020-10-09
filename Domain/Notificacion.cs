using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auriculoterapia.Api.Domain
{
    public class Notificacion
    {
        public int Id {get;set;}
        public int ReceptorId {get;set;}

        [JsonIgnore]
        [ForeignKey("ReceptorId")]
        public virtual Usuario Receptor {get;set;}
        public int EmisorId {get;set;}

        [JsonIgnore]
        [ForeignKey("EmisorId")]
        public virtual Usuario Emisor {get;set;}
        public string TipoNotificacion {get;set;}
        public bool Deshabilitado {get; set;}
        public DateTime FechaNotificacion {get; set;}
        public DateTime HoraNotificacion {get; set;}
        public string Titulo {get;set;}
        public string Descripcion {get;set;}
        public bool Leido {get; set;}
    
   }
}