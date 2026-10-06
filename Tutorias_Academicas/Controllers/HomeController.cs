using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Tutorias_Academicas.Data;

namespace Tutorias_Academicas.Controllers
{
    public class HomeController : Controller
    {
        private readonly TutoriasAcademicasContext _context = new TutoriasAcademicasContext();

        public ActionResult Index()
        {
            // Traer todas las tutorías con su maestro
            var tutorias = _context.Tutorias
                .Include(t => t.Maestro)
                .ToList();

            // Lista de IDs de tutorías en las que está inscrito el alumno logueado
            var tutoriasInscritas = new System.Collections.Generic.List<int>();

            if (Session["UserId"] != null && Session["Rol"] != null
                && Session["Rol"].ToString() == "Alumno")
            {
                int alumnoId = (int)Session["UserId"];

                tutoriasInscritas = _context.Inscripciones
                    .Where(i => i.AlumnoId == alumnoId)
                    .Select(i => i.TutoriaId)
                    .ToList();
            }

            ViewBag.TutoriasInscritas = tutoriasInscritas;

            return View(tutorias);  // el modelo sigue siendo IEnumerable<Tutoria>
        }
    }
}

