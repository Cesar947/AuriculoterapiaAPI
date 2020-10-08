using System.Collections.Generic;
using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Repository
{
    public interface INotificacionRepository:IRepository<Notificacion>
    {
        IEnumerable<Notificacion> getNotificacionByReceptorId(int receptorId);

        bool modificarDeshabilitar(int id);
         
    }
}