using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP_Pres_4_Grupo_D
{
    class Solicitud
    {
        const string solicitudes = "Solicitudes.txt";
        public int Registro { get; set; }
        public int CodCarrera { get; set; }
        public int CodMateria { get; set; }
        public int CodCursoOriginal { get; set; }
        public int CodCursoAlternativo { get; set; }
        public DateTime Fecha { get; set; }

        List<Solicitud> solicitud = new List<Solicitud>();

        public Solicitud() { }

        public Solicitud(int elRegistro, int laCarrera, int elCodMateria, int elCodCursoOriginal, int elCodCursoAlternativo, DateTime laFecha)
        {
            Registro = elRegistro;
            CodCarrera = laCarrera;
            CodMateria = elCodMateria;
            CodCursoOriginal = elCodCursoOriginal;
            CodCursoAlternativo = elCodCursoAlternativo;
            Fecha = laFecha;
        }

        public Solicitud(string linea)
        {
            var datos = linea.Split('|');
            Registro = int.Parse(datos[0]);
            CodCarrera = int.Parse(datos[1]);
            CodMateria = int.Parse(datos[2]);
            CodCursoOriginal = int.Parse(datos[3]);
            CodCursoAlternativo = int.Parse(datos[4]);
            Fecha = DateTime.Parse(datos[5]);
        }

        public void Levantar()
        {
            if (File.Exists(solicitudes))
            {
                using (var reader = new StreamReader(solicitudes))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        if (linea == "Registro|CodCarrera|CodMateria|CodCursoOriginal|CodCursoAlernativo|Fecha")
                        {
                            continue;
                        }
                        else
                        {
                            var unaSolicitud = new Solicitud(linea);
                            solicitud.Add(unaSolicitud);
                        }
                    }
                }
            }
        }

        public void Grabar()
        {

            using (var writer = new StreamWriter(solicitudes, append: false))
            {
                writer.WriteLine("Registro|CodCarrera|CodMateria|CodCursoOriginal|CodCursoAlernativo|Fecha");
                foreach (var sol in solicitud)
                {
                    var linea = sol.ObtenerLineaDatos();
                    writer.WriteLine(linea);
                }
            }
        }

        private object ObtenerLineaDatos() => $"{Registro}|{CodCarrera}|{CodMateria}|{CodCursoOriginal}|{CodCursoAlternativo}|{Fecha}";

        public bool GetSolicitud(int registro)
        {
            int p = 0;
            bool encontrado = false;
            while (p < solicitud.Count && !encontrado)
            {
                if (solicitud[p].Registro == registro)
                {
                    encontrado = true;
                }
                else
                {
                    p++;
                }
            }
            return encontrado;
        }

        public List<Solicitud> ListarSolicitud(int registro)
        {
            List<Solicitud> unaSol = new List<Solicitud>();
            foreach (Solicitud sol in solicitud)
            {
                if (sol.Registro == registro)
                {
                    unaSol.Add(sol);
                }
            }
            return unaSol;
        }

        public void IncorporarSolicitudes(List<Solicitud> laSolicitud)
        {
            foreach (Solicitud sol in laSolicitud)
            {
                solicitud.Add(sol);
            }
        }


    }
}
