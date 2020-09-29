using Auriculoterapia.Api.Domain;
using Auriculoterapia.Api.Helpers;
using Auriculoterapia.Api.Repository;
using System;
using System.Collections.Generic;


namespace Auriculoterapia.Api.Service.Implementation
{
    public class PacienteService: IPacienteService
    {
        private IPacienteRepository PacienteRepository;


        public PacienteService(IPacienteRepository PacienteRepository){
            this.PacienteRepository = PacienteRepository;
        }

        public void Save(Paciente entity){
            PacienteRepository.Save(entity);
        }

        public IEnumerable<Paciente> FindAll(){
            return PacienteRepository.FindAll();
        }

        public IEnumerable<Paciente> buscarPorPalabra(string palabra){
            return PacienteRepository.busquedaPacientePorPalabra(palabra);
        }

        public CantidadPacientesPorSexo retornarPacientesPorSexo(){
            return PacienteRepository.retornarPacientesPorSexo();
        }

        public CantidadPacientePorEdad retornarPacientesPorEdad(){
            return PacienteRepository.retornarPacientesPorEdad();
        }

        public List<ResponsePacientesObesidad> retornarCantidadPacientesPorEdadObesidad(string sexo){
            var listaResponse = new List<ResponsePacientesObesidad>();
            try{
                listaResponse.Add(PacienteRepository.retornarCantidadPacientesPorEdadObesidad(14, 17, sexo, "Adolescentes"));
                listaResponse.Add(PacienteRepository.retornarCantidadPacientesPorEdadObesidad(18, 30, sexo, "Jóvenes"));
                listaResponse.Add(PacienteRepository.retornarCantidadPacientesPorEdadObesidad(31, 45, sexo, "Adultos"));
                listaResponse.Add(PacienteRepository.retornarCantidadPacientesPorEdadObesidad(46, 60, sexo, "Adultos mayores"));
            } catch(Exception e){
                throw;
            }
            return listaResponse;
        }
    }
}