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
                paciente = this.context.Pacientes
                .Include(p => p.Usuario)
                .Single(paciente => paciente.Id == Id);
            }catch(System.Exception){
                throw;
            }
            return paciente;
        }

        public PacienteResultsParameters findResultParametersByPacienteId(int Id){
            var result = new PacienteResultsParameters();
            try{
                var paciente = this.context.Pacientes
                .Include(p => p.Usuario)
                .Single(p => p.Id == Id);

                result.sexo = paciente.Usuario.Sexo;
                result.edad = CalculoValores.calculoEdad(paciente.FechaNacimiento);

                
            }catch(System.Exception){
                throw;
            }
            return result;
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
               
                 var dbQueryHombres = this.context.Evoluciones
                .Include(e => e.Tratamiento.SolicitudTratamiento)
                .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                .Where(e => e.Tratamiento.SolicitudTratamiento.Paciente.Usuario.Sexo == "Masculino")
                .Select(e => new { pId = e.Tratamiento.SolicitudTratamiento.PacienteId})
                .ToList();

                var dbQueryMujeres = this.context.Evoluciones
                .Include(e => e.Tratamiento.SolicitudTratamiento)
                .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                .Where(e => e.Tratamiento.SolicitudTratamiento.Paciente.Usuario.Sexo == "Femenino")
                .Select(e => new { pId = e.Tratamiento.SolicitudTratamiento.PacienteId})
                .ToList();
                
                
                estadistica.cantidadHombres = dbQueryHombres.Distinct().Count();
                estadistica.cantidadMujeres = dbQueryMujeres.Distinct().Count();


            } catch(System.Exception){
                throw;
            }

            return estadistica;


        }

        public List<Paciente> pacientesPorEdad(int min, int max, string tratamiento="", string sexo=""){
            var result = new List<Paciente>();
            try{
                var pacientes = new List<Paciente>();
                if(tratamiento != ""){
                    if(sexo != ""){
                        pacientes = this.context.Evoluciones
                        .Include(e => e.Tratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                        
                        .Where(e => e.TipoTratamiento == tratamiento)
                        .Where(e => e.Tratamiento.SolicitudTratamiento.Paciente.Usuario.Sexo == sexo)
                        .Select(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                        .ToList();
                    } else{
                         pacientes = this.context.Evoluciones
                        .Include(e => e.Tratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                        
                        .Where(e => e.TipoTratamiento == tratamiento)
                        .Select(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                        .ToList();
                    }

                    

                } else{
                     pacientes = this.context.Evoluciones
                        .Include(e => e.Tratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                        
                        .Select(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                        .ToList(); 
                }
                
                if(pacientes != null || pacientes.Count() > 0){
                    foreach(var p in pacientes){
                        var edad = CalculoValores.calculoEdad(p.FechaNacimiento);
                        if(edad >= min && edad <= max){
                            result.Add(p);
                        }
                    }
                }
                
            } catch(Exception e){
                throw;
            }
            return result;

        }

        
        public List<double> calculoIMCyGCPromedio(List<Paciente> pacientes, string sexo){
            var IMCPromedio = 0.0;
            var GCPromedio = 0.0;
            var listaPromedios = new List<double>();
            try{

               if(pacientes.Count() > 0){
                        foreach(var p in pacientes){
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
                   
                
                        if(IMCPromedio != 0) listaPromedios.Add(IMCPromedio/(pacientes.Count()));
                        if(GCPromedio != 0) listaPromedios.Add(GCPromedio/(pacientes.Count()));
                     }
                    
                
                
            } catch(Exception e){
                throw;
            }
            return listaPromedios;
        }


        public CantidadPacientePorEdad retornarPacientesPorEdad(){
            var resultado = new CantidadPacientePorEdad();
            try{
                
                resultado.cantAdolescentes = pacientesPorEdad(14, 17).Distinct().Count();
                resultado.cantJovenes = pacientesPorEdad(18, 30).Distinct().Count();
                resultado.cantAdultos = pacientesPorEdad(31, 45).Distinct().Count();
                resultado.cantAdultosMayores = pacientesPorEdad(46, 60).Distinct().Count();

            } catch(Exception e){
                Console.WriteLine(e.Message);
            }
            return resultado;
        }

        public int calcularPacientesPorNivel(int nivel, string tratamiento)
        {
            var resultado = 0;
            try{
                var pacientes = this.context.Evoluciones
                        .Include(e => e.Tratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento)
                        .Include(e => e.Tratamiento.SolicitudTratamiento.Paciente)
                        .Where(e => e.TipoTratamiento == tratamiento)
                        .Select(e => e.Tratamiento.SolicitudTratamiento.Paciente).Distinct().ToList();

                foreach(var p in pacientes){
                    var sesionMaxima = this.context.Evoluciones
                                    .Include(e => e.Tratamiento)
                                    .Include(e => e.Tratamiento.SolicitudTratamiento)
                                    .Where(e => e.Tratamiento.SolicitudTratamiento.PacienteId == p.Id)
                                    .Select(e => e.Sesion).Max();

                    var evolucion = this.context.Evoluciones
                                    .Include(e => e.Tratamiento)
                                    .Include(e => e.Tratamiento.SolicitudTratamiento)
                                    .Where(e => e.Tratamiento.SolicitudTratamiento.PacienteId == p.Id)
                                    .Where(e => e.Sesion == sesionMaxima)
                                    .Where(e => e.EvolucionNumero == nivel);
                    if(evolucion != null){
                        resultado += 1;
                    }

                }
                        
                        
            } catch(Exception e){
                throw;
            }
            return resultado;

        }

        public CantidadPacientesPorNivel retornarPacientesPorNivel(string tratamiento){
            var resultado = new CantidadPacientesPorNivel();
            try{
               resultado.cantidadNivelUno = calcularPacientesPorNivel(1, tratamiento);

            } catch(Exception e){
                Console.WriteLine(e.Message);
            }


            return resultado;

        }

        public ResponsePacientesObesidad retornarCantidadPacientesPorEdadObesidad(int min, int max, string sexo, string tipoPacientePorEdad){
            var result = new ResponsePacientesObesidad();
        
            try{
                var pacientes = pacientesPorEdad(min, max, "Obesidad", sexo);
                result.Cantidad = pacientes.Distinct().Count();
                result.TipoPacientePorEdad = tipoPacientePorEdad;
              
                
                var IMCyGC = calculoIMCyGCPromedio(pacientes.Distinct().ToList(), sexo);
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
                    result.TipoIndicadorImc = "-";
                    result.TipoIndicadorGc = "-";
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
                //tipoIMC = "#F2BA52";
            } else if(IMC > 15 && IMC < 15.9){
                tipoIMC = "Delgadez severa";
                //tipoIMC = "#FDE289";
            } else if(IMC >= 16 && IMC <= 18.4){
                tipoIMC = "Delgadez";
                //tipoIMC = "#FEF0C1";
            } else if(IMC >= 18.5 && IMC <= 24.9){
               tipoIMC = "Peso saludable";
               // tipoIMC = "#D2E1CB";
            } else if(IMC >= 25 && IMC <= 29.9){
              tipoIMC = "Sobrepeso";
               // tipoIMC = "#F5C09E";
            } else if(IMC >= 30 && IMC <= 34.9){
               tipoIMC = "Obesidad severa";
               // tipoIMC = "#EEA070";
            } else if(IMC >= 40){
               tipoIMC = "Obesidad mórbida";
               // tipoIMC = "#B8450F";
            }

            return tipoIMC;
        }

        public string definirTipoGC(double GC, string sexo, string tipoPacientePorEdad){
             var tipoGC = "";
             if(sexo == "Masculino"){
                    if(tipoPacientePorEdad == "Adolescentes"
                        || tipoPacientePorEdad == "Jóvenes"){
                           if(GC <= 13){
                                tipoGC = "BUENA";
                            }
                            else if(GC > 13 && GC <= 20){
                                tipoGC = "NORMAL";
                            }
                            else if(GC > 20 && GC <= 23){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 23){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos"){
                            /* if(GC < 12){
                                tipoGC = "IDEAL";
                            } else*/ if(GC <= 14){
                                tipoGC = "BUENA";
                            }
                            else if(GC > 14 && GC <= 21){
                                tipoGC = "NORMAL";
                            }
                            else if(GC > 21 && GC <= 24){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 24){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos mayores"){
                        /*if(GC < 15){
                                tipoGC = "IDEAL";
                            } else*/ if(GC <= 17){
                                tipoGC = "BUENA";
                            }
                            else if(GC > 17 && GC <= 24){
                                tipoGC = "NORMAL";
                            }
                            else if(GC > 24 && GC <= 27){
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
                            } else*/ if(GC <= 19){
                                tipoGC = "BUENA";
                            }
                            else if(GC > 19 && GC <= 28){
                                tipoGC = "NORMAL";
                            }
                            else if(GC > 28 && GC <= 31){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 31){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos"){
                            /* if(GC < 17){
                                tipoGC = "IDEAL";
                            } else*/ if(GC <= 20){
                                tipoGC = "BUENA";
                            }
                            else if(GC > 20 && GC <= 29){
                                tipoGC = "NORMAL";
                            }
                            else if(GC > 29 && GC <= 32){
                                tipoGC = "ELEVADA";
                            }
                            else if(GC > 32){
                                tipoGC = "MUY ELEVADA";
                            }
                    } else if(tipoPacientePorEdad == "Adultos mayores"){
                        /*if(GC < 19){
                                tipoGC = "IDEAL";
                            } else*/ if(GC <= 22){
                                tipoGC = "BUENA";
                            }
                            else if(GC > 22 && GC <= 31){
                                tipoGC = "NORMAL";
                            }
                            else if(GC > 31 && GC <= 34){
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
