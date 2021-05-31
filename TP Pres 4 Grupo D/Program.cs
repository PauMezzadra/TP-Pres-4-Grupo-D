using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Pres_4_Grupo_D
{
    class Program
    {
        static void Main(string[] args)
        {
            CtrInscripcion elControl = new CtrInscripcion();
            elControl.LevantarArchivos();
            elControl.Iniciar();
        }
    }
}
