using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP_Pres_4_Grupo_D
{
    class MateriasAprobadas
    {
        const string materiasAprobadas = "MateriasAprobadas.txt";
        public int Registro { get; set; }
        public int CodMateria { get; set; }

        List<MateriasAprobadas> materias = new List<MateriasAprobadas>();
        List<MateriasAprobadas> subListado = new List<MateriasAprobadas>();

        public MateriasAprobadas() { }

        public MateriasAprobadas(string linea)
        {
            var datos = linea.Split('|');
            Registro = int.Parse(datos[0]);
            CodMateria = int.Parse(datos[1]);
        }

        public void Levantar()
        {
            if (File.Exists(materiasAprobadas))
            {
                using (var reader = new StreamReader(materiasAprobadas))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        if (linea == "Registro|CodMateria")
                        {
                            continue;
                        }
                        else
                        {
                            var unaMateriaAprobada = new MateriasAprobadas(linea);
                            materias.Add(unaMateriaAprobada);
                        }
                    }
                }
            }
        }

        public List<MateriasAprobadas> ListarMateriasAlumno(int registro)
        {
            foreach (MateriasAprobadas mat in materias)
            {
                if (mat.Registro == registro)
                {
                    subListado.Add(mat);
                }
            }
            return subListado;
        }

    }
}
