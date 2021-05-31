using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP_Pres_4_Grupo_D
{
    class PlanesEstudio
    {
        const string planesEstudio = "PlanesEstudio.txt";
        public int CodCarrera { get; set; }
        public string Carrera { get; set; }
        public int CodMateria { get; set; }
        public string Materia { get; set; }

        List<PlanesEstudio> planes = new List<PlanesEstudio>();
        List<PlanesEstudio> planParaOferta = new List<PlanesEstudio>();

        public PlanesEstudio() { }

        public PlanesEstudio(string linea)
        {
            var datos = linea.Split('|');
            CodCarrera = int.Parse(datos[0]);
            Carrera = datos[1];
            CodMateria = int.Parse(datos[2]);
            Materia = datos[3];
        }
        public void Levantar()
        {
            if (File.Exists(planesEstudio))
            {
                using (var reader = new StreamReader(planesEstudio))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        if (linea == "CodCarrera|Carrera|CodMateria|Materia")
                        {
                            continue;
                        }
                        else
                        {
                            var unPlan = new PlanesEstudio(linea);
                            planes.Add(unPlan);
                        }
                    }
                }
            }
        }

        public List<PlanesEstudio> ListarMateriasAcordePlan(int carrera)
        {
            foreach (PlanesEstudio plan in planes)
            {
                if (plan.CodCarrera == carrera)
                {
                    planParaOferta.Add(plan);
                }
            }
            return planParaOferta;
        }

    }
}
