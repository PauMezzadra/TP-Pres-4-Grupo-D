using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP_Pres_4_Grupo_D
{
    class Oferta
    {
        const string ofertaCursos = "OfertaCursos.txt";
        public int CodMateria { get; set; }
        public int CodCurso { get; set; }
        public string Materia { get; set; }
        public string Titular { get; set; }
        public string Docente { get; set; }
        public string Dias { get; set; }
        public string Horario { get; set; }
        public string Sede { get; set; }

        List<Oferta> oferta = new List<Oferta>();
        List<Oferta> ofertaLimitadaMateria = new List<Oferta>();

        public Oferta() { }

        public Oferta(string linea)
        {
            var datos = linea.Split('|');
            CodMateria = int.Parse(datos[0]);
            CodCurso = int.Parse(datos[1]);
            Materia = datos[2];
            Titular = datos[3];
            Docente = datos[4];
            Dias = datos[5];
            Horario = datos[6];
            Sede = datos[7];
        }

        public void Levantar()
        {
            if (File.Exists(ofertaCursos))
            {
                using (var reader = new StreamReader(ofertaCursos))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        if (linea == "CodMateria|CodCurso|NombreMateria|Titular|Docente|Dias|Horario|Sede")
                        {
                            continue;
                        }
                        else
                        {
                            var unaOferta = new Oferta(linea);
                            oferta.Add(unaOferta);
                        }
                    }
                }
            }
        }

        public List<Oferta> LimitarOfertaMateria(int codMateria)
        {
            foreach (Oferta ofer in oferta)
            {
                if (ofer.CodMateria == codMateria)
                {
                    ofertaLimitadaMateria.Add(ofer);
                }
            }
            return ofertaLimitadaMateria;
        }

        public List<Oferta> ObtenerDatos(int codMateria, int codCurso)
        {
            List<Oferta> datos = new List<Oferta>();
            foreach (Oferta of in oferta)
            {
                if (of.CodMateria == codMateria && of.CodCurso == codCurso)
                {
                    datos.Add(of);
                }
            }
            return datos;
        }

    }
}
