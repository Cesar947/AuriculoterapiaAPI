using System;
using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Repository.Context;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Auriculoterapia.Api.Helpers;
using MySql.Data.MySqlClient;

namespace Auriculoterapia.Api.Repository.Implementation
{
    public class PacienteRepository: IPacienteRepository
    {
        private ApplicationDbContext context;

        private readonly IUsuarioRepository usuarioRepository;


        public PacienteRepository(ApplicationDbContext context,IUsuarioRepository usuarioRepository)
        {
            this.context = context;
            this.usuarioRepository = usuarioRepository;
        }

        public IEnumerable<Paciente> FindAll(){
            var pacientes = new List<Paciente>();
            try{
                pacientes = this.context.Pacientes.Include(p => p.Usuario).ToList();
            }catch(System.Exception){
                    throw;
            }
            return pacientes;
        }
        public void Save(Paciente entity){
            try{

                var isValidEmail = usuarioRepository.IsValidEmail(entity.Usuario.Email); 
                
                if(isValidEmail){
                        usuarioRepository.Save(entity.Usuario);
                        if(entity.Usuario.Id > 0){

                        DateTime birth = DateTime.Parse(entity.FechaNacimiento.ToString());
                        DateTime today = DateTime.Today;
                        int edad = today.Year - birth.Year;
                        //int age = int.Parse(edad.ToString());

                        if (today.Month < birth.Month ||
                        ((today.Month == birth.Month) && (today.Day < birth.Day)))
                        {
                            edad--;
                        }

                        entity.Edad = edad;
                        context.Add(entity);
                        context.SaveChanges();
                    }
                }
                else{
                    entity.Id = -1;
                    Console.WriteLine("Correo Inválido");
                }
                
            }catch{
                throw;
            }
            
        }

        
        public Paciente FindById(int Id){
            var paciente = new Paciente();
            try{
                paciente = this.context.Pacientes.Single(paciente => paciente.Id == Id);
            }catch(System.Exception){
                throw;
            }
            return paciente;
        }


        public Paciente buscarPorUsuarioId(int usuarioId){
                var paciente = new Paciente();
            try{
                paciente = this.context.Pacientes.Single(paciente => paciente.UsuarioId == usuarioId);
            }catch(System.Exception){
                throw;
            }
            return paciente;
        }



        public string ActualizarNumeroPaciente(string numero, Paciente paciente){
            string actualizado = "Actualizado";
            try{
                paciente.Celular = numero;
                this.context.SaveChanges();
            } catch{

            }
            return actualizado;
        }

        public IEnumerable<Paciente> busquedaPacientePorPalabra(string palabras){
           var pacientes = new List<Paciente>();
            try{
                /*pacientes = (from p in this.context.Pacientes
                            where (terminos.Any(r  => p.Usuario.Nombre.Contains(r))) ||
                                  (terminos.Any(r  => p.Usuario.Apellido.Contains(r)))
                            select p).ToList();*/
                var dbListPacientes = from s in this.context.SolicitudTratamientos
                                        join p in this.context.Pacientes on s.PacienteId equals p.Id
                                        select p;
                                        
                if (!String.IsNullOrEmpty(palabras)){
                    dbListPacientes = dbListPacientes.Distinct().Where(p => (p.Usuario.Nombre + " " + p.Usuario.Apellido).Contains(palabras));
                }
                pacientes = dbListPacientes.Distinct().Include(p => p.Usuario).ToList();
                
            } catch(System.Exception){
                throw;
            }
            return pacientes;

        }

        public CantidadPacientesPorSexo retornarPacientesPorSexo(){
            var estadistica = new CantidadPacientesPorSexo();
     
            try{
               
                 var dbQueryHombres = this.context.Tratamientos
                .Include(t => t.SolicitudTratamiento)
                .Where(t => t.SolicitudTratamiento.Paciente.Usuario.Sexo == "Masculino")
                .Select(t => new { pId = t.SolicitudTratamiento.PacienteId})
                .ToList();

                var dbQueryMujeres = this.context.Tratamientos
                .Include(t => t.SolicitudTratamiento)
                .Where(t => t.SolicitudTratamiento.Paciente.Usuario.Sexo == "Femenino")
                .Select(t => new { pId = t.SolicitudTratamiento.PacienteId})
                .ToList();
                
                
                estadistica.cantidadHombres = dbQueryHombres.Distinct().Count();
                estadistica.cantidadMujeres = dbQueryMujeres.Distinct().Count();


            } catch(System.Exception){
                throw;
            }

            return estadistica;


        }

        public int cantidadPacientesPorEdad(int min, int max, string tratamiento=""){
            var cantidad = 0;
            try{
                if(tratamiento != ""){
                    cantidad = this.context.Tratamientos
                    .Include(t => t.SolicitudTratamiento)
                    .Include(t => t.SolicitudTratamiento.Paciente)
                    .Where(t => t.SolicitudTratamiento.Paciente.Edad >= min
                    && t.SolicitudTratamiento.Paciente.Edad <= max)
                    .Where(t => t.TipoTratamiento == tratamiento)
                    .Select(t => new {pId = t.SolicitudTratamiento.Paciente.Id})
                    .ToList().Distinct().Count(); 

                } else{
                    cantidad = this.context.Tratamientos
                    .Include(t => t.SolicitudTratamiento)
                    .Include(t => t.SolicitudTratamiento.Paciente)
                    .Where(t => t.SolicitudTratamiento.Paciente.Edad >= min
                    && t.SolicitudTratamiento.Paciente.Edad <= max)
                    .Select(t => new {pId = t.SolicitudTratamiento.Paciente.Id})
                    .ToList().Distinct().Count(); 
                }
                
            } catch(Exception e){
                throw;
            }
            return cantidad;

        }

        
        public List<double> calculoIMCyGCPromedio(string sexo, int min, int max, string tratamiento){
            var IMCPromedio = 0.0;
            var GCPromedio = 0.0;
            var listaPromedios = new List<double>();
            try{
               
               var pacientes = this.context.Tratamientos
                                .Include(t => t.SolicitudTratamiento)
                                .Include(t => t.SolicitudTratamiento.Paciente)
                                .Include(t => t.SolicitudTratamiento.Paciente.Usuario)
                                .Where(t => t.SolicitudTratamiento.Paciente.Usuario.Sexo == sexo)
                                .Where(t => t.TipoTratamiento == tratamiento)
                                .Select(t => t.SolicitudTratamiento.Paciente)
                                .ToList();

                if(pacientes.Count > 0){
                    var pacientesPorEdad = new List<Paciente>();
                    foreach(var p in pacientes){
                        var edad = CalculoValores.calculoEdad(p.FechaNacimiento);
                        if(edad >= min && edad <= max){
                            pacientesPorEdad.Add(p);
                        }
                    }

                    if(pacientesPorEdad.Count() > 0){
                        foreach(var p in pacientesPorEdad){
                            var sesionMaxima = this.context.Evoluciones
                                    .Include(e => e.Tratamiento)
                                    .Include(e => e.Tratamiento.SolicitudTratamiento)
                                    .Where(e => e.Tratamiento.SolicitudTratamiento.PacienteId == p.Id)
                                    .Select(e => e.Sesion).Max();
                            
                            var evolucion = this.context.Evoluciones
                                        .Include(e => e.Tratamiento)
                                        .Include(e => e.Tratamiento.SolicitudTratamiento)
                                        .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                                        .Where(e => e.Tratamiento.SolicitudTratamiento.Paciente.Id == p.Id)
                                        .Where(e => e.Sesion == sesionMaxima).FirstOrDefault();

                                var altura = evolucion.Tratamiento.SolicitudTratamiento.Altura;
                                var peso = evolucion.Peso;
                                var IMC = CalculoValores.calculoIMC(
                                    altura,
                                    peso);
                                int edad = CalculoValores.calculoEdad(evolucion.Tratamiento.SolicitudTratamiento.Paciente.FechaNacimiento);
                                var GC = CalculoValores.calculoGC(
                                    IMC, edad, sexo
                                );

                                IMCPromedio += IMC;
                                GCPromedio += GC;
                                    
                        }
                   
                
                        if(IMCPromedio != 0) listaPromedios.Add(IMCPromedio/(pacientesPorEdad.Count()));
                        if(GCPromedio != 0) listaPromedios.Add(GCPromedio/(pacientesPorEdad.Count()));
                     }
                    }
                
                
            } catch(Exception e){
                throw;
            }
            return listaPromedios;
        }


        public CantidadPacientePorEdad retornarPacientesPorEdad(){
            var resultado = new CantidadPacientePorEdad();
            try{
                
                resultado.cantAdolescentes = cantidadPacientesPorEdad(14, 17);
                resultado.cantJovenes = cantidadPacientesPorEdad(18, 30);
                resultado.cantAdultos = cantidadPacientesPorEdad(31, 45);
                resultado.cantAdultosMayores = cantidadPacientesPorEdad(46, 60);

            } catch(Exception e){
                Console.WriteLine(e.Message);
            }
            return resultado;
        }


        public CantidadPacientesPorNivel retornarPacientesPorNivel(){
            var resultado = new CantidadPacientesPorNivel();
            try{
               var evolucionesObesidad = this.context.Evoluciones
                                .Include(e => e.Tratamiento)
                                .Include(e => e.Tratamiento.SolicitudTratamiento)
                                .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                                .Where(e => e.TipoTratamiento == "Obesidad")
                                .OrderByDescending(e => e.TipoTratamiento)
                                .OrderByDescending(e => e.Sesion)
                                .Select(e => new {
                                    tipoTratamiento = e.TipoTratamiento,
                                    sesion = e.Sesion,
                                    pId = e.Tratamiento.SolicitudTratamiento.PacienteId,
                                    nivel = e.EvolucionNumero}
                                ).ToList();

            } catch(Exception e){
                Console.WriteLine(e.Message);
            }


            return resultado;

        }

        public ResponsePacientesObesidad retornarCantidadPacientesPorEdadObesidad(int min, int max, string sexo, string tipoPacientePorEdad){
            var result = new ResponsePacientesObesidad();
        
            try{
                result.Cantidad = cantidadPacientesPorEdad(min, max, "Obesidad");
                result.TipoPacientePorEdad = tipoPacientePorEdad;
              
                
                var IMCyGC = calculoIMCyGCPromedio(sexo, min, max, "Obesidad");
                if(IMCyGC != null && IMCyGC.Count() > 0){
                    result.ImcPromedio = IMCyGC[0];
                    result.PorcentajeGcPromedio = IMCyGC[1];

                    result.TipoIndicadorImc = definirTipoIMC(result.ImcPromedio);
                    result.TipoIndicadorGc = definirTipoGC(
                        result.PorcentajeGcPromedio,
                        sexo,
                        result.TipoPacientePorEdad);
                } else{
                    result.ImcPromedio = 0;
                    result.PorcentajeGcPromedio = 0;
                    result.TipoIndicadorImc = "";
                    result.TipoIndicadorGc = "";
                }
                

            } catch(Exception e){
                Console.WriteLine(e.Message);
            }
            return result;
        }


        public string definirTipoIMC(double IMC){
            var tipoIMC = "";
            if (IMC <= 15){
                tipoIMC = "Delgadez muy severa";
            } else if(IMC > 15 && IMC < 15.9){
                tipoIMC = "Delgadez severa";
            } else if(IMC >= 16 && IMC <= 18.4){
                tipoIMC = "Delgadez";
            } else if(IMC >= 18.5 && IMC <= 24.9){
                tipoIMC = "Peso saludable";
            } else if(IMC >= 25 && IMC <= 29.9){
                tipoIMC = "Sobrepeso";
            } else if(IMC >= 30 && IMC <= 34.9){
                tipoIMC = "Obesidad severa";
            } else if(IMC >= 40){
                tipoIMC = "Obesidad mórbida";
            }

            return tipoIMC;
        }

        public string definirTipoGC(double GC, string sexo, string tipoPacientePorEdad){
             var tipoGC = "";
             if(sexo == "Masculino"){
                    if(tipoPacientePorEdad == "Adolescentes"
                        || tipoPacientePorEdad == "Jóvenes"){
                            /*if(GC < 11){
                                tipoGC = "IDEAL";
                            } else*/ if(GC >= 11 && GC <= 13){
                                tipoGC = "BUENA";
                            }
                            else if(GC >= 14 && GC <= 20){
                                tipoGC = "NORMAL";
                            }
                            else if(GC >= 21 && GC <= 23){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 23){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos"){
                            /* if(GC < 12){
                                tipoGC = "IDEAL";
                            } else*/ if(GC >= 12 && GC <= 14){
                                tipoGC = "BUENA";
                            }
                            else if(GC >= 15 && GC <= 21){
                                tipoGC = "NORMAL";
                            }
                            else if(GC >= 22 && GC <= 24){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 24){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos mayores"){
                        /*if(GC < 15){
                                tipoGC = "IDEAL";
                            } else*/ if(GC >= 15 && GC <= 17){
                                tipoGC = "BUENA";
                            }
                            else if(GC >= 18 && GC <= 24){
                                tipoGC = "NORMAL";
                            }
                            else if(GC >= 25 && GC <= 27){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 27){
                                tipoGC = "MUY ELEVADA";
                            }
                    }
                } else if(sexo == "Femenino"){
                     if(tipoPacientePorEdad == "Adolescentes"
                        || tipoPacientePorEdad == "Jóvenes"){
                            /*if(GC < 16){
                                tipoGC = "IDEAL";
                            } else*/ if(GC >= 16 && GC <= 19){
                                tipoGC = "BUENA";
                            }
                            else if(GC >= 20 && GC <= 28){
                                tipoGC = "NORMAL";
                            }
                            else if(GC >= 29 && GC <= 31){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 31){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos"){
                            /* if(GC < 17){
                                tipoGC = "IDEAL";
                            } else*/ if(GC >= 17 && GC <= 20){
                                tipoGC = "BUENA";
                            }
                            else if(GC >= 21 && GC <= 29){
                                tipoGC = "NORMAL";
                            }
                            else if(GC >= 30 && GC <= 32){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 32){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos mayores"){
                        /*if(GC < 19){
                                tipoGC = "IDEAL";
                            } else*/ if(GC >= 19 && GC <= 22){
                                tipoGC = "BUENA";
                            }
                            else if(GC >= 18 && GC <= 24){
                                tipoGC = "NORMAL";
                            }
                            else if(GC >= 32 && GC <= 34){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 34){
                                tipoGC = "MUY ELEVADA";
                            }
                    }
                }
                return tipoGC;
        }


    }
}
