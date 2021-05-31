using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Pres_4_Grupo_D
{
    class CtrInscripcion
    {
        public CtrInscripcion() { }

        PlanesEstudio unPlan = new PlanesEstudio();
        Alumno unAlumno = new Alumno();
        Oferta unaOferta = new Oferta();
        Correlatividades unaCorrelativa = new Correlatividades();
        MateriasAprobadas unaMateria = new MateriasAprobadas();
        Solicitud unaSolicitud = new Solicitud();

        const string opcionIngresar = "I";
        const string opcionConsultar = "C";
        const string opcionSalir = "S";
        string opcion;

        int registro;
        string apellidoNombre;
        string ultimasCuatro;
        int carrera;
        int codMateria;
        int cursoOriginal;
        int cursoAlternativo;
        string continuar;

        List<PlanesEstudio> planCarrera = new List<PlanesEstudio>(); //Lista de las materias acordes a la carrera seleccionada por el alumno
        List<PlanesEstudio> planAlumno = new List<PlanesEstudio>(); //Lista de las materias que el alumno aún no aprobó
        List<Oferta> ofertaCarrera = new List<Oferta>();
        List<MateriasAprobadas> materiasAprobAlumno = new List<MateriasAprobadas>(); //Lista de las materias aprobadas por el alumno
        List<Correlatividades> losRequisitos = new List<Correlatividades>(); //Lista de los requisitos de las materias faltantes del alumno (correlatividades)
        List<PlanesEstudio> planParaMostrar = new List<PlanesEstudio>(); //Lista de las materias en las que el alumno se puede anotar porque cumple con los requisitos
        List<Solicitud> nuevaSolicitud = new List<Solicitud>(); //Lista de las solicitudes realizadas en el proceso actual
        Solicitud laSolicitud;


        public void Iniciar() //Logueo del usuario
        {
            do
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Bienvenido al Sitio de la FCE - UBA");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine();
                opcion = Validaciones.PedirStrNoVac($"Ingrese la opción deseada:\n" + "\n" +
                                                        $"{opcionIngresar} - Ingresar al Sistema de Gestión\n" +
                                                            $"{opcionSalir} - Salir");

                switch (opcion)
                {
                    case opcionIngresar:

                        registro = Validaciones.PedirInt("\nIngrese su número de registro", 100000, 999999);
                        if (!unAlumno.GetAlumno(registro))
                        {
                            Console.WriteLine($"\nEl número ingresado ({registro}) no corresponde a un alumno de la Facultad");
                        }
                        else
                        {
                            apellidoNombre = unAlumno.GetNombre(registro);
                            Console.WriteLine($"\nBienvenido/a: {apellidoNombre}\n");
                            Ingresar();
                        }
                        break;

                    case opcionSalir:
                        break;

                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                }

            } while (opcion != opcionSalir);
        }

        public void Ingresar()
        {
            do
            {
                opcion = Validaciones.PedirStrNoVac("\nIngrese la opción deseada:\n" + "\n" +
                                                    "C - Consultar Inscripciones\n" +
                                                    "I - Realizar Inscripción\n" +
                                                    "S - Salir\n");
                switch (opcion)
                {
                    case opcionConsultar:
                        ConsultarSolicitudesPrevias(registro);
                        break;

                    case opcionIngresar:
                        LimpiarListas();
                        if (unaSolicitud.GetSolicitud(registro)) //Chequea si tiene registrada una solicitud de inscripción previa
                        {
                            Console.WriteLine("\nUd. ya tiene registrada una solicitud de inscripción.\n" +
                                                "Para realizar una nueva inscripción, deberá esperar al segundo llamado.\n");
                        }
                        else
                        {
                            if (!unAlumno.GetCondicion(registro)) //Chequea si está en condición de alumno regular
                            {
                                Console.WriteLine("\nUd. no se encuentra como Alumno Regular y por lo tanto no puede inscribirse. " +
                                                        "Contáctese con el Depto. de Alumnos\n");
                            }
                            else
                            {
                                ultimasCuatro = Validaciones.PedirSoN("Se encuentra Ud. en las últimas cuatro materias de su carrera? 'S / N'"); //Preguntamos si está en últimas 4
                                if (ultimasCuatro == "S")
                                {
                                    carrera = SeleccionarCarrera();
                                    ArmarListadoMaterias(carrera, registro); //Me arma la lista planAlumno, con las materias que le faltan de la carrera seleccionada
                                    if (planAlumno.Count > 4)
                                    {
                                        Console.WriteLine("\nA Ud. le faltan más de cuatro materias para la carrera seleccionada");
                                        LimpiarListas();
                                        break;
                                    }
                                    else
                                    {
                                        for (int i = 0; i < 4; i++)
                                        {
                                            LimpiarListas();
                                            ArmarListadoMaterias(carrera, registro);
                                            IngresoInscripcionUltimasCuatro(planAlumno, carrera, registro);

                                            if (i < 3)
                                            {
                                                continuar = Validaciones.PedirSoN("\nDesea continuar inscribiéndose en otra materia? 'S / N' ");
                                                if (continuar == "N")
                                                {
                                                    i = 4;
                                                }
                                            }
                                        }
                                        GrabarSolicitud();
                                    }

                                }
                                else
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        LimpiarListas();
                                        IngresoInscripcion();
                                        if (i < 2)
                                        {
                                            continuar = Validaciones.PedirSoN("\nDesea continuar inscribiéndose en otra materia? 'S / N' ");
                                            if (continuar == "N")
                                            {
                                                i = 3;
                                            }
                                        }
                                    }
                                    GrabarSolicitud();

                                }
                            }
                        }

                        break;

                    case opcionSalir:
                        break;

                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                }

            } while (opcion != opcionSalir);
        }

        public void LevantarArchivos()
        {
            unPlan.Levantar();
            unAlumno.Levantar();
            unaOferta.Levantar();
            unaCorrelativa.Levantar();
            unaMateria.Levantar();
            unaSolicitud.Levantar();
        }

        public int SeleccionarCarrera()
        {
            int carrera = Validaciones.PedirInt("\nIngrese la carrera:\n" +
                                                "\n1 - Sistemas\n" +
                                                "2 - Contador\n" +
                                                "3 - Administración\n" +
                                                "4 - Economía\n" +
                                                "5 - Actuario en Administración\n" +
                                                "6 - Actuario en Economía\n", 1, 6);
            return carrera;

        }

        public void ConsultarSolicitudesPrevias(int registro)
        {
            List<Solicitud> unaSol = new List<Solicitud>();
            List<Oferta> unaOfer = new List<Oferta>();
            int mat = 0;
            int cursOr = 0;
            int cursAlt = 0;

            if (!unaSolicitud.GetSolicitud(registro))
            {
                Console.WriteLine("Ud. no tiene registradas solicitudes de inscripción en este llamado");
            }
            else
            {
                unaSol = unaSolicitud.ListarSolicitud(registro);
                Console.WriteLine("\nLos cursos que Ud. tiene registrados en su solicitud son:\n");
                foreach (var sol in unaSol)
                {
                    mat = sol.CodMateria;
                    cursOr = sol.CodCursoOriginal;
                    cursAlt = sol.CodCursoAlternativo;

                    unaOfer = unaOferta.ObtenerDatos(mat, cursOr);
                    foreach (var o in unaOfer)
                    {
                        if (o.CodCurso == cursOr)
                        {
                            Console.WriteLine($"Materia: {(o.Materia).ToUpper()}");
                            Console.WriteLine("--------------------------------------------------------------------------------");
                            Console.WriteLine($"Curso ORIGINAL: ({o.CodMateria} - {o.CodCurso})\n" +
                                              $"Docente: {o.Docente} - Días: {o.Dias} - Horario: {o.Horario} - Sede: {o.Sede}\n");
                        }
                    }
                    unaOfer.Clear();
                    unaOfer = unaOferta.ObtenerDatos(mat, cursAlt);
                    foreach (var o in unaOfer)
                    {
                        if (o.CodCurso == cursAlt)
                        {
                            Console.WriteLine($"Curso ALTERNATIVO: ({o.CodMateria} - {o.CodCurso})\n" +
                                              $"Docente: {o.Docente} - Días: {o.Dias} - Horario: {o.Horario} - Sede: {o.Sede}\n\n");
                        }
                    }
                    unaOfer.Clear();
                }


            }
        }

        public void IngresoInscripcion()
        {
            carrera = SeleccionarCarrera();
            ArmarListadoMaterias(carrera, registro); //Me arma la lista planAlumno, con las materias que le faltan de la carrera seleccionada
            VerificarRequisitos(carrera, planAlumno, materiasAprobAlumno, nuevaSolicitud);
            do //Seleccionamos la materia
            {
                codMateria = Validaciones.PedirInt("\nIngrese el código de la materia en la que desea anotarse:\n"); //Pedimos el código de la materia
                if (!VerificarMateria(planParaMostrar, codMateria))
                {
                    Console.WriteLine("\nDebe seleccionar un código de MATERIA de los que se muestran junto al listado de materias\n");
                }
            } while (!VerificarMateria(planParaMostrar, codMateria));

            ListarCursos(codMateria);
            SeleccionarCursos(codMateria);
            laSolicitud = ArmarSolicitud(registro, carrera, codMateria, cursoOriginal, cursoAlternativo);
            Console.WriteLine("\nSu elección de curso ORIGINAL es:\n");
            ListarCursos(codMateria, cursoOriginal);
            Console.WriteLine("\nSu elección de curso ALTERNATIVO es:\n");
            ListarCursos(codMateria, cursoAlternativo);
            ConfirmarSolicitud();
        }

        public void IngresoInscripcionUltimasCuatro(List<PlanesEstudio> planAlumno, int carrera, int registro)
        {
            foreach (var p in planAlumno)
            {
                planParaMostrar.Add(p);
            }
            foreach (var p in planParaMostrar)
            {
                if (!FindMateriaEnSolicitud(nuevaSolicitud, p.CodMateria))
                {
                    Console.WriteLine($"Código: {p.CodMateria} - {p.Materia}");
                }
            }
            do //Seleccionamos la materia
            {
                codMateria = Validaciones.PedirInt("\nIngrese el código de la materia en la que desea anotarse:\n"); //Pedimos el código de la materia
                if (!VerificarMateria(planParaMostrar, codMateria))
                {
                    Console.WriteLine("\nDebe seleccionar un código de MATERIA de los que se muestran junto al listado de materias\n");
                }
            } while (!VerificarMateria(planParaMostrar, codMateria));

            ListarCursos(codMateria);
            SeleccionarCursos(codMateria);
            laSolicitud = ArmarSolicitud(registro, carrera, codMateria, cursoOriginal, cursoAlternativo);
            Console.WriteLine("\nSu elección de curso ORIGINAL es:\n");
            ListarCursos(codMateria, cursoOriginal);
            Console.WriteLine("\nSu elección de curso ALTERNATIVO es:\n");
            ListarCursos(codMateria, cursoAlternativo);
            ConfirmarSolicitud();
        }

        private void ArmarListadoMaterias(int carrera, int registro)
        {
            planCarrera = unPlan.ListarMateriasAcordePlan(carrera);
            materiasAprobAlumno = unaMateria.ListarMateriasAlumno(registro);
            foreach (var materia in planCarrera)
            {
                if (!FindMateria(materiasAprobAlumno, materia.CodMateria))
                {
                    planAlumno.Add(materia);
                }
            }
        }

        private bool FindMateriaEnSolicitud(List<Solicitud> nuevaSolicitud, int codMateria)
        {
            bool encontrado = false;
            foreach (var s in nuevaSolicitud)
            {
                if (s.CodMateria == codMateria)
                {
                    encontrado = true;
                }
            }
            return encontrado;
        }
        private bool FindMateria(List<MateriasAprobadas> lasMateriasAprobadas, int codMateria)
        {
            bool encontrado = false;
            foreach (var mat in lasMateriasAprobadas)
            {
                if (mat.CodMateria == codMateria)
                {
                    encontrado = true;
                }
            }
            return encontrado;
        }

        private void VerificarRequisitos(int carrera, List<PlanesEstudio> planDelAlumno, List<MateriasAprobadas> lasMateriasAprobadas, List<Solicitud> nuevaSolicitud)
        {
            List<int> requisitosFaltantes = new List<int>();
            losRequisitos = unaCorrelativa.GetRequisitos(carrera, planDelAlumno); //Obtengo lista de las materias faltantes con sus requisitos
            foreach (var r in losRequisitos)
            {
                if (!FindRequisito(lasMateriasAprobadas, r.Requisito))
                {
                    requisitosFaltantes.Add(r.CodMateria); //Me da las materias de las que NO cumplo con los requisitos
                }
            }
            foreach (var p in planAlumno)
            {
                if (!GetMateria(requisitosFaltantes, p.CodMateria))
                {
                    planParaMostrar.Add(p);
                }
            }
            foreach (var p in planParaMostrar)
            {
                if (!FindMateriaEnSolicitud(nuevaSolicitud, p.CodMateria))
                {
                    Console.WriteLine($"Código: {p.CodMateria} - {p.Materia}");
                }

            }
        }

        private bool FindRequisito(List<MateriasAprobadas> lasMateriasAprobadas, int requisito)
        {
            bool encontrado = false;
            foreach (var mat in lasMateriasAprobadas)
            {
                if (mat.CodMateria == requisito)
                {
                    encontrado = true;
                }
            }
            return encontrado;
        }

        private bool GetMateria(List<int> lasQueNoPuedoAnotarme, int codigo)
        {
            bool encontrado = false;
            foreach (var p in lasQueNoPuedoAnotarme)
            {
                if (p == codigo)
                {
                    encontrado = true;
                }
            }
            return encontrado;
        }

        public bool VerificarMateria(List<PlanesEstudio> planParaMostrar, int codMateria) //Verifica que la materia seleccionada esté entre las opciones
        {
            int p = 0;
            bool encontrado = false;
            while (p < planParaMostrar.Count && !encontrado)
            {
                if (planParaMostrar[p].CodMateria == codMateria)
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

        private void ListarCursos(int codMateria) //Lista los cursos disponibles para la materia elegida
        {
            ofertaCarrera = unaOferta.LimitarOfertaMateria(codMateria);
            foreach (Oferta of in ofertaCarrera)
            {
                Console.WriteLine($"\nCódigo: {of.CodMateria} - Materia: {of.Materia} || Curso: {of.CodCurso}\n");
                Console.WriteLine($"Docente: {of.Docente} - Días: {of.Dias} - Horario: {of.Horario} - Sede: {of.Sede}\n");
                Console.WriteLine("------------------------------------------------------------------------------------\n");

            }
        }

        private void ListarCursos(int codMateria, int curso)
        {
            ofertaCarrera.Clear();
            ofertaCarrera = unaOferta.LimitarOfertaMateria(codMateria);
            foreach (Oferta of in ofertaCarrera)
            {
                if (of.CodMateria == codMateria && of.CodCurso == curso)
                {
                    Console.WriteLine($"\nCódigo: {of.CodMateria} - Materia: {of.Materia} || Curso: {of.CodCurso}\n");
                    Console.WriteLine($"Docente: {of.Docente} - Días: {of.Dias} - Horario: {of.Horario} - Sede: {of.Sede}\n");
                    Console.WriteLine("------------------------------------------------------------------------------------\n");
                }
            }
        }

        public void SeleccionarCursos(int codMateria) //Selecciona los cursos en los que se quiere anotar
        {
            do
            {
                cursoOriginal = Validaciones.PedirInt("\nIngrese el número de curso que elige como 'Curso ORIGINAL'\n"); //Pedimos que elija curso original
                if (!VerificarCurso(ofertaCarrera, codMateria, cursoOriginal))
                {
                    Console.WriteLine("\nDebe seleccionar un código de CURSO de los que se muestran junto al listado de materias\n");
                }
            } while (!VerificarCurso(ofertaCarrera, codMateria, cursoOriginal));

            do
            {
                cursoAlternativo = Validaciones.PedirInt("\nIngrese el número de curso que elige como 'Curso ALTERNATIVO'.\n" +
                                                         "Si no desea elegir curso alternativo, ingrese 0 (cero)\n");//Pedimos que elija curso alternativo
                if (cursoAlternativo == 0)
                {
                    break;
                }
                else
                {
                    if (!VerificarCurso(ofertaCarrera, codMateria, cursoAlternativo))
                    {
                        Console.WriteLine("\nDebe seleccionar un código de CURSO de los que se muestran junto al listado de materias\n");
                    }
                    else
                    {
                        if (cursoAlternativo == cursoOriginal)
                        {
                            Console.WriteLine("El curso ALTERNATIVO no puede ser igual que el curso ORIGINAL");
                        }
                    }
                }

            } while ((!VerificarCurso(ofertaCarrera, codMateria, cursoAlternativo)) || (cursoOriginal == cursoAlternativo));

        }

        private bool VerificarCurso(List<Oferta> ofertaCarrera, int codMateria, int codCurso) //verifica que los cursos seleccionados estén disponibles
        {
            int p = 0;
            bool encontrado = false;
            while (p < ofertaCarrera.Count && !encontrado)
            {
                if (ofertaCarrera[p].CodMateria == codMateria && ofertaCarrera[p].CodCurso == codCurso)
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

        public Solicitud ArmarSolicitud(int registro, int carrera, int materia, int cursoOriginal, int cursoAlternativo)
        {
            DateTime fecha = DateTime.Now;
            Solicitud estaSolicitud = new Solicitud(registro, carrera, materia, cursoOriginal, cursoAlternativo, fecha);
            return estaSolicitud;
        }

        public void ConfirmarSolicitud()
        {
            string opcion;
            opcion = Validaciones.PedirSoN("\nConfirma la selección? 'S / N'\n");
            if (opcion == "N")
            {
                LimpiarListas();
                Console.WriteLine("Su selección no ha sido guardada");
            }
            else if (opcion == "S")
            {
                nuevaSolicitud.Add(laSolicitud);
            }
        }

        public void GrabarSolicitud()
        {
            unaSolicitud.IncorporarSolicitudes(nuevaSolicitud); //Incorporo la solicitud realizada a la lista de solicitudes (de la clase Solicitud) que va a ser grabada
            unaSolicitud.Grabar(); //Grabo, guardando la solicitud realizada
            LimpiarListas();
            Console.WriteLine("\nSu solicitud de inscripción ha sido registrada con éxito.\n");
        }

        public void LimpiarListas()
        {
            ofertaCarrera.Clear();
            planAlumno.Clear();
            planCarrera.Clear();
            planParaMostrar.Clear();
        }

    }
}
