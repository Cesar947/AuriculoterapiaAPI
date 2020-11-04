using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository;
using Auriculoterapia.Api.Helpers;
using System.Collections.Generic;


namespace Auriculoterapia.Api.Service.Implementation
{
    public class UsuarioService: IUsuarioService
    {
        private IUsuarioRepository UsuarioRepository;


        public UsuarioService(IUsuarioRepository UsuarioRepository){
            this.UsuarioRepository = UsuarioRepository;
        }

        public void Save(Usuario entity){
            UsuarioRepository.Save(entity);
        }

        public IEnumerable<Usuario> FindAll(){
           return UsuarioRepository.FindAll();
        }

        public Response Autenticar(string nombreUsuario, string password){
            return UsuarioRepository.Autenticar(nombreUsuario,password);
        }

        public Usuario FinbyId(int id){
            return UsuarioRepository.FindById(id);
        }

        public ResponseActualizarPassword actualizar_Contrasena(string nombreUsuario,string palabraClave, string password){
            return UsuarioRepository.actualizar_Contrasena(nombreUsuario,palabraClave,password);
        
        }

        public ResponseUsuarioById BuscarPorId(int userId){
            return UsuarioRepository.BuscarPorId(userId);
        }

        public ResponseActualizarKeyWord Actualizar_KeyWord(int idUser,string palabraClave,string nuevaPalabraClave){
            return UsuarioRepository.Actualizar_KeyWord(idUser,palabraClave,nuevaPalabraClave);
        }

        public ResponseActualizarFoto Actualizar_Foto(int idUser,string foto){
            return UsuarioRepository.Actualizar_Foto(idUser,foto);
        }

        public ResponseActualizarFoto Buscar_Foto(int idUser){
            return UsuarioRepository.Buscar_Foto(idUser);
        }
        
        public ResponseValidationEmail ValidateEmailCode(int idUser,string code){
            return UsuarioRepository.ValidateEmailCode(idUser,code);
        }

        public ResponseValidationEmail BuscarValidationEmailUser(int idUser, string correo){
            return UsuarioRepository.BuscarValidationEmailUser(idUser,correo);
        }
    }   
}