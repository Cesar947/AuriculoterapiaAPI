using System;
using System.Linq;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Auriculoterapia.Api.Helpers;


using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Auriculoterapia.Api.Repository.Implementation
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private ApplicationDbContext context;
        private readonly IConfiguration _config;
        private readonly IRol_UsuarioRepository rol_UsuarioRepository;

        public UsuarioRepository(ApplicationDbContext context, IConfiguration config,IRol_UsuarioRepository rol_UsuarioRepository)
        {
            this.context = context;
            this._config = config;
            this.rol_UsuarioRepository = rol_UsuarioRepository;
        }

        public IEnumerable<Usuario> FindAll(){
            var users = new List<Usuario>();
            try{
                users = context.Usuarios.Include(u => u.Paciente).ToList();
            }
            catch(System.Exception)
            {
                throw;
            }
            return users;
        }
        /*public void Save(Usuario entity){
            try{
                context.Usuarios.Add(entity);
                context.SaveChanges();
            }
            catch{
                throw;
            }

        }*/

        public void Save(Usuario entity){
            
            try{
                var user = context.Usuarios.FirstOrDefault(x => x.NombreUsuario == entity.NombreUsuario
                || x.Contrasena == entity.Contrasena);

               var emailExsist = context.Usuarios.FirstOrDefault(x => x.Email == entity.Email);

               if(emailExsist != null){
                   entity.Id = -1;
               }
                
                if(user == null && emailExsist == null){
                    context.Usuarios.Add(entity);

                    context.SaveChanges();
                    rol_UsuarioRepository.Asignar_Usuario_Rol(entity);
                }
              
            }
            catch{
                throw;
            }

        }

        public Response Autenticar(string nombreUsuario, string password){
            var user = context.Usuarios.FirstOrDefault(x =>x.NombreUsuario == nombreUsuario
            && x.Contrasena == password);
          

            if(user == null)
                return null;

            var rol = context.Rol_Usuarios.Include(x =>x.Rol).FirstOrDefault(x =>x.UsuarioId == user.Id);    

            var tokenHelper = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, rol.Rol.Descripcion)

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHelper.CreateToken(tokenDescriptor);
            user.Token = tokenHelper.WriteToken(token);
            context.SaveChanges();

            user.Contrasena = null;


            return new Response(user.Id,user.NombreUsuario,user.Token, rol.Rol.Descripcion,user.Nombre,user.Apellido);

        }


        public void Asignar_Paciente(Usuario usuario){
            try{
                context.Pacientes.Add(usuario.Paciente);
                context.SaveChanges();
                
            }
            catch{
                throw;
            }

        }

        public ResponseActualizarPassword actualizar_Contrasena(string nombreUsuario,string palabraClave, string password){
         
            var user = context.Usuarios.FirstOrDefault(x =>x.NombreUsuario == nombreUsuario
            && x.PalabraClave == palabraClave);

            if(user == null){
                return null;
            }
            if(user.Contrasena == password){
                user.Contrasena = "SAME";
                return new ResponseActualizarPassword(user.NombreUsuario,user.PalabraClave,user.Contrasena);
            }

            user.Contrasena=password;
            context.SaveChanges();
            user.Contrasena=null;
            return new ResponseActualizarPassword(user.NombreUsuario,user.PalabraClave,user.Contrasena);

        }


        public Usuario FindById(int id){
            var usuario = context.Usuarios.FirstOrDefault(x => x.Id == id);
            var rol = context.Rol_Usuarios.Include(x =>x.Rol).FirstOrDefault(x =>x.UsuarioId == usuario.Id);

            
            if(rol.Rol.Descripcion == "paciente"){
                if(usuario != null)
                usuario.Contrasena = null;

                return usuario;
            }
            //return Console.Error("ff");
            throw new Exception("$No pertenece al rol paciente");
                        
        }

        public ResponseUsuarioById BuscarPorId(int userId){
            var usuario = context.Usuarios.Include(p => p.Paciente).
            Include(e => e.Especialista).FirstOrDefault(x => x.Id == userId);
            var rol = context.Rol_Usuarios.Include(x =>x.Rol).FirstOrDefault(x =>x.UsuarioId == usuario.Id);
            var paciente = context.Pacientes.FirstOrDefault(x => x.UsuarioId == userId);
            var especialista = context.Especialistas.FirstOrDefault(x => x.UsuarioId == userId);

            ResponseUsuarioById responseUsuario;

            

            if (rol.Rol.Descripcion == "PACIENTE"){

                if(usuario != null)
                {
                    DateTime birth = DateTime.Parse(paciente.FechaNacimiento.ToString());
                    DateTime today = DateTime.Today;
                    int edad = today.Year - birth.Year;
                    //int age = int.Parse(edad.ToString());

                    if (today.Month < birth.Month ||
                    ((today.Month == birth.Month) && (today.Day < birth.Day)))
                    {
                        edad--;
                    }


                    usuario.Contrasena = null;

                    responseUsuario = new ResponseUsuarioById(usuario.Id,usuario.Nombre,
                    usuario.Apellido,usuario.Email,usuario.Contrasena,
                    usuario.NombreUsuario,usuario.Sexo,usuario.PalabraClave,
                    paciente.FechaNacimiento,edad);

                    return responseUsuario;
                }
            }else{
                if(usuario != null){
                    usuario.Contrasena = null;
                    Nullable<DateTime> nullDate = null;

                    responseUsuario =  new ResponseUsuarioById(usuario.Id,usuario.Nombre,
                    usuario.Apellido,usuario.Email,usuario.Contrasena,
                    usuario.NombreUsuario,usuario.Sexo,usuario.PalabraClave,
                    nullDate,null);

                    return responseUsuario;
                }
                
            }
            return null;
        }

        public ResponseActualizarKeyWord Actualizar_KeyWord(int idUser,string palabraClave,string nuevaPalabraClave){
            var usuario = context.Usuarios.Include(p => p.Paciente).
                Include(e => e.Especialista).FirstOrDefault(x => x.Id == idUser && x.PalabraClave == palabraClave);
            
            ResponseActualizarKeyWord keyWord;

            if(palabraClave == nuevaPalabraClave && usuario != null){
                return new ResponseActualizarKeyWord(usuario.Id,"SAME","SAME");
            }

            if(usuario != null ){
                usuario.PalabraClave = nuevaPalabraClave;
                context.SaveChanges();
                usuario.PalabraClave = "";
                keyWord = new ResponseActualizarKeyWord(usuario.Id,usuario.PalabraClave,"");
            }else{
                keyWord = null;
            }    
            return keyWord;
        }

        public bool IsValidEmail(string email){
            if(string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }    
        }

    }
}
