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
    [Authorize] // solo exige estar logueado
    public class TutoriasController : Controller
    {
        private readonly TutoriasAcademicasContext _context = new TutoriasAcademicasContext();

        // GET: Tutorias
        public ActionResult Index()
        {
            if ((string)Session["Rol"] != "Maestro")
                return RedirectToAction("Index", "Home");

            var tutorias = _context.Tutorias
                .Include(t => t.Maestro)
                .ToList();

        
            return View(tutorias);
        }


        // GET: Tutorias/Crear

        public ActionResult Crear()
        {
            if ((string)Session["Rol"] != "Maestro")
                return RedirectToAction("Index", "Home");

            // Cargar lista de maestros
            var maestros = _context.Usuarios
                .Where(u => u.Rol.Nombre == "Maestro")
                .ToList();

            ViewBag.Maestros = new SelectList(
                maestros,
                "Id",
                "NombreCompleto"
            );

            return View();
        }


        //Post: Tutorias/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Tutoria t)
        {
            // Solo maestros pueden entrar aquí
            if ((string)Session["Rol"] != "Maestro")
                return RedirectToAction("Index", "Home");

            //  VALIDAR QUE SE HAYA SELECCIONADO UN MAESTRO 
            if (t.MaestroId == 0)
            {
                ModelState.AddModelError("MaestroId", "Debe seleccionar un maestro.");

                var maestrosLista = _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Rol.Nombre == "Maestro")
                    .ToList();

                ViewBag.Maestros = new SelectList(maestrosLista, "Id", "NombreCompleto", t.MaestroId);
                return View(t);
            }

            // VALIDAR QUE EL USUARIO SELECCIONADO REALMENTE SEA MAESTRO
            var maestro = _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Id == t.MaestroId);

            if (maestro == null || maestro.Rol == null || maestro.Rol.Nombre != "Maestro")
            {
                ModelState.AddModelError("MaestroId", "El usuario seleccionado no es un maestro válido.");

                var maestrosLista = _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Rol.Nombre == "Maestro")
                    .ToList();

                ViewBag.Maestros = new SelectList(maestrosLista, "Id", "NombreCompleto", t.MaestroId);
                return View(t);
            }

            // ALIDAR QUE ESE MAESTRO NO TENGA OTRA TUTORÍA EN EL MISMO HORARIO 
            bool horarioOcupado = _context.Tutorias.Any(tu =>
                tu.MaestroId == t.MaestroId &&
                tu.Horario == t.Horario
            );

            if (horarioOcupado)
            {
                ModelState.AddModelError("",
                    "El maestro seleccionado no está disponible en el horario indicado. Por favor, selecciona otro horario.");

                var maestrosLista = _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Rol.Nombre == "Maestro")
                    .ToList();

                ViewBag.Maestros = new SelectList(maestrosLista, "Id", "NombreCompleto", t.MaestroId);
                return View(t);
            }

            // SI TODO ESTÁ BIEN, GUARDAR
            _context.Tutorias.Add(t);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }





        // GET: Tutorias/Editar/5
        public ActionResult Editar(int id)
        {
            if ((string)Session["Rol"] != "Maestro")
                return RedirectToAction("Index", "Home");

            var tutoria = _context.Tutorias.Find(id);
            if (tutoria == null)
                return HttpNotFound();

            // 👉 LLENAR SIEMPRE EL COMBO DE MAESTROS
            var maestrosLista = _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Rol.Nombre == "Maestro")
                .ToList();

            ViewBag.Maestros = new SelectList(maestrosLista, "Id", "NombreCompleto", tutoria.MaestroId);

            return View(tutoria);
        }


        // POST: Tutorias/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Tutoria t)
        {
            if ((string)Session["Rol"] != "Maestro")
                return RedirectToAction("Index", "Home");

            //  VALIDAR SELECCIÓN DE MAESTRO
            if (t.MaestroId == 0)
            {
                ModelState.AddModelError("MaestroId", "Debe seleccionar un maestro.");

                var maestrosLista = _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Rol.Nombre == "Maestro")
                    .ToList();

                ViewBag.Maestros = new SelectList(maestrosLista, "Id", "NombreCompleto", t.MaestroId);
                return View(t);
            }

            //  VALIDAR QUE EL USUARIO SEA MAESTRO
            var maestro = _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Id == t.MaestroId);

            if (maestro == null || maestro.Rol == null || maestro.Rol.Nombre != "Maestro")
            {
                ModelState.AddModelError("MaestroId", "El usuario seleccionado no es un maestro válido.");

                var maestrosLista = _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Rol.Nombre == "Maestro")
                    .ToList();

                ViewBag.Maestros = new SelectList(maestrosLista, "Id", "NombreCompleto", t.MaestroId);
                return View(t);
            }

            // VALIDAR QUE NO EXISTA OTRA TUTORÍA DEL MISMO MAESTRO EN EL MISMO HORARIO
            bool horarioOcupado = _context.Tutorias.Any(tu =>
                tu.MaestroId == t.MaestroId &&
                tu.Horario == t.Horario &&
                tu.Id != t.Id   // Ignorar la misma tutoria
            );

            if (horarioOcupado)
            {
                ModelState.AddModelError("",
                    "El maestro seleccionado no está disponible en el horario indicado. Por favor, selecciona otro horario.");

                var maestrosLista = _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Rol.Nombre == "Maestro")
                    .ToList();

                ViewBag.Maestros = new SelectList(maestrosLista, "Id", "NombreCompleto", t.MaestroId);
                return View(t);
            }

            // 4) ACTUALIZAR LA TUTORÍA
            var tutoriaDb = _context.Tutorias.Find(t.Id);
            if (tutoriaDb == null)
                return HttpNotFound();

            tutoriaDb.Materia = t.Materia;
            tutoriaDb.Descripcion = t.Descripcion;
            tutoriaDb.Horario = t.Horario;
            tutoriaDb.Cupo = t.Cupo;
            tutoriaDb.Modalidad = t.Modalidad;
            tutoriaDb.Aula = t.Aula;
            tutoriaDb.MaestroId = t.MaestroId;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Tutorias/Eliminar/5
        public ActionResult Eliminar(int id)
        {
            if ((string)Session["Rol"] != "Maestro")
                return RedirectToAction("Index", "Home");

            var tutoria = _context.Tutorias.Find(id);
            if (tutoria == null)
                return HttpNotFound();

            // Contar cuántos alumnos inscritos tiene esta tutoría
            var cantidadInscritos = _context.Inscripciones
                .Count(i => i.TutoriaId == id);

            // Si NO hay inscritos -> borrar directo
            if (cantidadInscritos == 0)
            {
                _context.Tutorias.Remove(tutoria);
                _context.SaveChanges();

                TempData["Mensaje"] = "Tutoría eliminada correctamente.";
                return RedirectToAction("Index");
            }

            // Si SÍ hay inscritos pedir confirmación
            ViewBag.CantidadInscritos = cantidadInscritos;
            return View(tutoria); // devuelve la vista Eliminar.cshtml
        }

        // POST: Tutorias/EliminarConfirmado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(int id)
        {
            if ((string)Session["Rol"] != "Maestro")
                return RedirectToAction("Index", "Home");

            var tutoria = _context.Tutorias.Find(id);
            if (tutoria == null)
                return HttpNotFound();

            // Borrar inscripciones primero
            var inscripciones = _context.Inscripciones
                .Where(i => i.TutoriaId == id)
                .ToList();

            if (inscripciones.Any())
            {
                _context.Inscripciones.RemoveRange(inscripciones);
            }

            _context.Tutorias.Remove(tutoria);
            _context.SaveChanges();

            TempData["Mensaje"] = "Tutoría y sus inscripciones fueron eliminadas correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Tutorias/Detalles/5
        public ActionResult Detalles(int id)
        {
            // Solo maestro
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Maestro")
                return RedirectToAction("Index", "Home");

            int maestroId = (int)Session["UserId"];

            // Cargar tutoría + maestro + inscripciones + alumnos
            var tutoria = _context.Tutorias
                .Include(t => t.Maestro)
                .Include(t => t.Inscripciones.Select(ins => ins.Alumno))
                .FirstOrDefault(t => t.Id == id && t.MaestroId == maestroId);

            if (tutoria == null)
                return HttpNotFound();

            return View(tutoria);  // Vista: Views/Tutorias/Detalles.cshtml
        }

        // POST: Tutorias/EliminarAlumno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarAlumno(int inscripcionId, int tutoriaId)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Maestro")
                return RedirectToAction("Index", "Home");

            int maestroId = (int)Session["UserId"];

            // Verificar que la inscripción exista y que la tutoría sea del maestro logueado
            var inscripcion = _context.Inscripciones
                .Include(i => i.Tutoria)
                .FirstOrDefault(i => i.Id == inscripcionId);

            if (inscripcion == null || inscripcion.Tutoria.MaestroId != maestroId)
            {
                TempData["Mensaje"] = "No se pudo encontrar la inscripción.";
                return RedirectToAction("Detalles", new { id = tutoriaId });
            }

            _context.Inscripciones.Remove(inscripcion);
            _context.SaveChanges();

            TempData["Mensaje"] = "Alumno eliminado de la tutoría.";
            return RedirectToAction("Detalles", new { id = tutoriaId });
        }


    }
}