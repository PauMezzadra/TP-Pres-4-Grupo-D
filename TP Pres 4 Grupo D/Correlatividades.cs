using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP_Pres_4_Grupo_D
{
    class Correlatividades
    {
        const string correlatividades = "Correlatividades.txt";
        public int CodCarrera { get; set; }
        public int CodMateria { get; set; }
        public int Requisito { get; set; }

        List<Correlatividades> correlativas = new List<Correlatividades>();
        List<Correlatividades> correlativasEspecificas = new List<Correlatividades>();
        public Correlatividades() { }

        public Correlatividades(string linea)
        {
            var datos = linea.Split('|');
            CodCarrera = int.Parse(datos[0]);
            CodMateria = int.Parse(datos[1]);
            Requisito = int.Parse(datos[2]);
        }

        public void Levantar()
        {
            if (File.Exists(correlatividades))
            {
                using (var reader = new StreamReader(correlatividades))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        if (linea == "CodCarrera|CodMateria|Requisito")
                        {
                            continue;
                        }
                        else
                        {
                            var unaCorrelativa = new Correlatividades(linea);
                            correlativas.Add(unaCorrelativa);
                        }
                    }
                }
            }
        }

        public List<Correlatividades> GetRequisitos(int codCarrera, List<PlanesEstudio> planAlumno)
        {
            foreach (PlanesEstudio pla in planAlumno)
            {
                foreach (Correlatividades corr in correlativas)
                {
                    if (corr.CodCarrera == codCarrera && corr.CodMateria == pla.CodMateria)
                    {
                        correlativasEspecificas.Add(corr);
                    }
                }
            }
            return correlativasEspecificas;
        }



    }
}
