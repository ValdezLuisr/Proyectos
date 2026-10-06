using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Tutorias_Academicas.Data;
using Tutorias_Academicas.Models;

namespace Tutorias_Academicas.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly TutoriasAcademicasContext _context = new TutoriasAcademicasContext();

        public ActionResult Index()
        {
            // Validar sesión
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Auth");

            int userId = (int)Session["UserId"];

            // Traer usuario con su rol
            var usuario = _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Id == userId);

            if (usuario == null)
                return HttpNotFound();

            var vm = new PerfilViewModel
            {
                Usuario = usuario
            };

            // Si es alumno: mostrar tutorías inscritas
            if (usuario.Rol.Nombre == "Alumno")
            {
                vm.TutoriasInscritas = _context.Inscripciones
                    .Where(i => i.AlumnoId == userId)
                    .Include(i => i.Tutoria)
                    .Include(i => i.Tutoria.Maestro)
                    .Select(i => i.Tutoria)
                    .ToList();
            }
            // Si es maestro: mostrar tutorías que imparte
            else if (usuario.Rol.Nombre == "Maestro")
            {
                vm.TutoriasAsignadas = _context.Tutorias
                    .Where(t => t.MaestroId == userId)
                    .Include(t => t.Maestro)
                    .ToList();
            }

            return View(vm);
        }
    }
}

