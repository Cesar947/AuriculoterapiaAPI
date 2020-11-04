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
               } else if(entity.Contrasena == entity.PalabraClave){
                   entity.Id = -2;
               } else if(entity.NombreUsuario == entity.Contrasena || entity.NombreUsuario == entity.PalabraClave){
                   entity.Id = -3;
               }
                
                if(user == null && emailExsist == null){              
                   if( entity.Id >=0){

                        var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                        var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,myTimeZone);

                        DateTime today = currentDateTime;
                        var codigo = new Random().Next(10000,100000);

                        entity.EmailExist = false;   
                        entity.FechaEnvioCorreoConfirmacion = today;
                        entity.Codigo = codigo.ToString();
                        context.Usuarios.Add(entity);
                        context.SaveChanges();
                        rol_UsuarioRepository.Asignar_Usuario_Rol(entity);
                        var correo = new SendEmail();
                        correo.sendEmailTo(entity.Nombre,entity.Email,"Codigo de confirmacion para Auriculoterapia",entity.Codigo);
                   }
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

            var userContrasena = context.Usuarios.FirstOrDefault(x => x.Contrasena == password);

            if(user == null){
                return null;
            }
            if(user.Contrasena == password){
                user.Contrasena = "SAME";
                return new ResponseActualizarPassword(user.NombreUsuario,user.PalabraClave,user.Contrasena);
            }
            if(userContrasena != null){
                user.Contrasena = "EXISTENTE";
                return new ResponseActualizarPassword(user.NombreUsuario,user.PalabraClave,user.Contrasena);
            }
            if(user.PalabraClave == password){
                user.Contrasena = "SAMEKEYWORD";
                return new ResponseActualizarPassword(user.NombreUsuario,user.PalabraClave,user.Contrasena);
            }

            if(user.NombreUsuario == password){
                user.Contrasena = "SAMEUSER";
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
                    paciente.FechaNacimiento,edad,paciente.Id);

                    return responseUsuario;
                }
            }else{
                if(usuario != null){
                    usuario.Contrasena = null;
                    Nullable<DateTime> nullDate = null;

                    responseUsuario =  new ResponseUsuarioById(usuario.Id,usuario.Nombre,
                    usuario.Apellido,usuario.Email,usuario.Contrasena,
                    usuario.NombreUsuario,usuario.Sexo,usuario.PalabraClave,
                    nullDate,null,null);

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

            if(nuevaPalabraClave == usuario.NombreUsuario && usuario != null){
                return new ResponseActualizarKeyWord(usuario.Id,"SAMEUSER","SAMEUSER");
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


        public ResponseValidationEmail ValidateEmailCode(int idUser,string code){
            var usuario = context.Usuarios.FirstOrDefault(x => x.Id == idUser);

            ResponseValidationEmail validationEmail;

            var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,myTimeZone);

            DateTime today = currentDateTime;
            var envioCorreo = usuario.FechaEnvioCorreoConfirmacion;

            var nuevaFecha = currentDateTime.Subtract(envioCorreo);

            var todayDate = today.Date;
            var envioCorreoDate = envioCorreo.Date;

            var todayHour = today.Hour;
            var envioCorreoHour = envioCorreo.Hour;

            var minutos = (today - envioCorreo).TotalMinutes;

            if(minutos < 60){
                if(usuario.Codigo == code){
                    usuario.EmailExist = true;
                    usuario.FechaConfirmacionCodigo = today;
                    context.SaveChanges();

                    validationEmail = new ResponseValidationEmail(usuario.Id,envioCorreo,today,usuario.Codigo,usuario.EmailExist);
                }else{
                    validationEmail = new ResponseValidationEmail(-1,envioCorreo,today,usuario.Codigo,usuario.EmailExist);

                }   
            }else{
                    validationEmail = new ResponseValidationEmail(-2,envioCorreo,today,usuario.Codigo,usuario.EmailExist);

            }
            
            

        
            return validationEmail;
            
        }

        public ResponseValidationEmail BuscarValidationEmailUser(int idUser, string nombreUsuario){
            var usuario = context.Usuarios.FirstOrDefault(x => x.Id == idUser || x.NombreUsuario == nombreUsuario);
            ResponseValidationEmail validationEmail;

            if(usuario != null){
                validationEmail = new ResponseValidationEmail(usuario.Id,null,null,usuario.Codigo,usuario.EmailExist);

            }else{
                validationEmail = new ResponseValidationEmail(-1,null,null,"codigo",false);
            }

            return validationEmail;
        }


        public ResponseActualizarFoto Actualizar_Foto(int idUser,string foto){
            var usuario = context.Usuarios.Include(p => p.Paciente).
                Include(e => e.Especialista).FirstOrDefault(x => x.Id == idUser);

            ResponseActualizarFoto Photo;

            if(usuario != null){
                string fotoWithTransformation = foto.Replace("upload/","upload/w_400,h_400,c_crop,g_face,r_max/w_200/");
                usuario.Foto = fotoWithTransformation;
                context.SaveChanges();
                Photo = new ResponseActualizarFoto(usuario.Id,usuario.Foto);
            }else{
                Photo = null;
            }

            return Photo;
        }

        public ResponseActualizarFoto Buscar_Foto(int idUser){
             var usuario = context.Usuarios.Include(p => p.Paciente).
                Include(e => e.Especialista).FirstOrDefault(x => x.Id == idUser);

            ResponseActualizarFoto Photo;

            if(usuario != null){
                Photo = new ResponseActualizarFoto(usuario.Id,usuario.Foto);
            }else{
                Photo = null;
            }
            return Photo;
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
