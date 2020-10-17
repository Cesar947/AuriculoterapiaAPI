using System.Collections.Generic;
using Auriculoterapia.Api.Domain;

namespace Auriculoterapia.Api.Service

{
    public interface INotificacionService:IService<Notificacion>
    {
        IEnumerable<Notificacion> getNotificacionByReceptorId(int receptorId);

         bool modificarDeshabilitar(int id);

         int numeroDeNotificacionesPorReceptorId(int id);

         bool leerNotificacionesPorReceptorId(int receptorId);

    }
}