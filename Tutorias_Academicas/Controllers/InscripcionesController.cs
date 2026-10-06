using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tutorias_Academicas.Data;
using System.Data.Entity;
using Tutorias_Academicas.Models;


namespace Tutorias_Academicas.Controllers
{
    // Solo exige estar autenticado (logueado)
    [Authorize]
    public class InscripcionesController : Controller
    {
        private readonly TutoriasAcademicasContext _context = new TutoriasAcademicasContext();

        public ActionResult Inscribir(int id)
        {
            // 1) Validar que haya sesión
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // 2) Validar que el rol sea Alumno
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Alumno")
            {
                TempData["Mensaje"] = "Solo los alumnos pueden inscribirse a tutorías.";
                return RedirectToAction("Index", "Home");
            }

            int alumnoId = (int)Session["UserId"];

            // 3) Verificar si ya está inscrito en esta tutoría
            bool yaInscritoEnEsta = _context.Inscripciones
                .Any(i => i.TutoriaId == id && i.AlumnoId == alumnoId);

            if (yaInscritoEnEsta)
            {
                TempData["Mensaje"] = "Ya estás inscrito en esta tutoría.";
                return RedirectToAction("Index", "Home");
            }

            // 4) Contar cuántas tutorías tiene inscritas este alumno
            int totalInscritas = _context.Inscripciones
                .Count(i => i.AlumnoId == alumnoId);

            if (totalInscritas >= 5)
            {
                TempData["Mensaje"] = "Ya estás inscrito en el máximo de 5 tutorías.";
                return RedirectToAction("Index", "Home");
            }

            // 5) Verificar capacidad de la tutoría
            var tutoria = _context.Tutorias.Find(id);
            if (tutoria == null)
            {
                TempData["Mensaje"] = "La tutoría no existe.";
                return RedirectToAction("Index", "Home");
            }

            int inscritosEnEsta = _context.Inscripciones
                .Count(i => i.TutoriaId == id);

            if (inscritosEnEsta >= tutoria.Cupo)
            {
                TempData["Mensaje"] = "Limite alcanzado.";
                return RedirectToAction("Index", "Home");
            }


            // 6) Crear nueva inscripción
            var ins = new Inscripcion
            {
                AlumnoId = alumnoId,
                TutoriaId = id
            };

            _context.Inscripciones.Add(ins);
            _context.SaveChanges();

            TempData["Mensaje"] = "¡Tutoría inscrita exitosamente!";
            return RedirectToAction("Index", "Home");
        }
    }
}