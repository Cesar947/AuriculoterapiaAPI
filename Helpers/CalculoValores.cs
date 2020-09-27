using System;

namespace Auriculoterapia.Api.Helpers
{
    public class CalculoValores
    {
       public static double calculoIMC(float altura, float peso){
           double IMC = peso/(altura*altura);
           return IMC;
       }

       public static double calculoGC(double IMC, int edad, string sexo){
           double grasaCorporal = 0.0;
           if(sexo=="Masculino"){
                grasaCorporal = 1.2*IMC+(0.23*edad)-(10.8*1)-5.4;
            }else{
                grasaCorporal = 1.2*IMC+(0.23*edad)-(10.8*0)-5.4;
            }
            return grasaCorporal;
       }

       public static int calculoEdad(DateTime birth){
        
            DateTime today = DateTime.Today;
            int edad = today.Year - birth.Year;
        
            if (today.Month < birth.Month ||
            ((today.Month == birth.Month) && (today.Day < birth.Day)))
            {
                edad--;
            }
            return edad;
       }
    }
}