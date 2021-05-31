using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TP_Pres_4_Grupo_D
{
    static class Validaciones
    {
        public static string PedirStrNoVac(string mensaje)
        {
            string valor;
            do
            {
                Console.WriteLine(mensaje);
                valor = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(valor))
                {
                    Console.WriteLine("No puede ser vacío");
                }
                valor = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(valor.ToUpper());
            } while (valor == "");

            return valor;
        }

        public static string PedirSoN(string mensaje)
        {
            string valor = "";
            string mensError = "Debe ingresar S o N";
            do
            {
                valor = PedirStrNoVac(mensaje);
                if (valor != "S" && valor != "N")
                {
                    Console.WriteLine(mensError);
                }
            } while (valor != "S" && valor != "N");

            return valor;
        }

        public static int PedirInt(string mensaje, int min, int max)
        {
            int valor;
            bool valido = false;
            string mensError = "Debe ingresar un valor entre " + min + " y " + max;

            do
            {
                Console.WriteLine(mensaje);
                if (!int.TryParse(Console.ReadLine(), out valor))
                {
                    Console.WriteLine(mensError);
                }
                else
                {
                    if (valor < min || valor > max)
                    {
                        Console.WriteLine(mensError);
                    }
                    else
                    {
                        valido = true;
                    }
                }
            } while (!valido);

            return valor;
        }

        public static int PedirInt(string mensaje)
        {
            int valor;
            bool valido = false;
            string mensError = "Debe ingresar un número";

            do
            {
                Console.WriteLine(mensaje);
                if (!int.TryParse(Console.ReadLine(), out valor))
                {
                    Console.WriteLine(mensError);
                }
                else
                {
                    valido = true;
                }
            } while (!valido);

            return valor;
        }
    }
}
