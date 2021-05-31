using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP_Pres_4_Grupo_D
{
    class Alumno
    {
        const string alumno = "Alumnos.txt";
        public int Registro { get; set; }
        public string ApellidoNombre { get; set; }
        public int CodCarrera { get; set; }
        public bool CondRegular { get; set; }

        List<Alumno> alumnos = new List<Alumno>();

        public Alumno() { }

        public Alumno(string linea)
        {
            var datos = linea.Split('|');
            Registro = int.Parse(datos[0]);
            ApellidoNombre = datos[1];
            CodCarrera = int.Parse(datos[2]);
            if (datos[3] == "Si")
            {
                CondRegular = true;
            }
            else
            {
                CondRegular = false;
            }
        }

        public void Levantar()
        {
            if (File.Exists(alumno))
            {
                using (var reader = new StreamReader(alumno))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        if (linea == "Registro|ApellidoNombre|CodCarrera|CondRegular")
                        {
                            continue;
                        }
                        else
                        {
                            var unAlumno = new Alumno(linea);
                            alumnos.Add(unAlumno);
                        }
                    }
                }
            }
        }

        public bool GetAlumno(int registro)
        {
            int p = 0;
            bool encontrado = false;
            while (p < alumnos.Count && !encontrado)
            {
                if (alumnos[p].Registro == registro)
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

        public string GetNombre(int registro)
        {
            int p = 0;
            bool encontrado = false;
            string elNombre = "";
            while (p < alumnos.Count && !encontrado)
            {
                if (alumnos[p].Registro == registro)
                {
                    elNombre = alumnos[p].ApellidoNombre;
                    encontrado = true;
                }
                else
                {
                    p++;
                }
            }
            return elNombre;
        }

        public bool GetCondicion(int registro)
        {
            int p = 0;
            bool encontrado = false;
            bool condicion = false;
            while (p < alumnos.Count && !encontrado)
            {
                if (alumnos[p].Registro == registro)
                {
                    condicion = alumnos[p].CondRegular;
                    encontrado = true;
                }
                else
                {
                    p++;
                }
            }
            return condicion;

        }
    }
}
