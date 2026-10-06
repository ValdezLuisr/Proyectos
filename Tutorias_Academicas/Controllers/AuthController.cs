using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tutorias_Academicas.Data;
using Tutorias_Academicas.Models;
namespace Tutorias_Academicas.Controllers
{
    public class AuthController : Controller
    {
        private readonly TutoriasAcademicasContext _context = new TutoriasAcademicasContext();

        // GET: Auth/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            // Si ya está logueado, podríamos redirigir según rol
            if (Session["UserId"] != null)
            {
                if ((string)Session["Rol"] == "Maestro")
                    return RedirectToAction("Index", "Tutorias");

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string correo, string contraseña)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contraseña))
                {
                    ViewBag.Error = "Debes ingresar correo y contraseña.";
                    return View();
                }

                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(x => x.Correo == correo);

                if (usuario == null)
                {
                    ViewBag.Error = "Usuario no encontrado.";
                    return View();
                }

                if (string.IsNullOrEmpty(usuario.ContrasenaHash))
                {
                    ViewBag.Error = "El usuario no tiene contraseña configurada.";
                    return View();
                }

                if (usuario.ContrasenaHash != contraseña)
                {
                    ViewBag.Error = "Contraseña incorrecta.";
                    return View();
                }

                // Autenticación correcta
                FormsAuthentication.SetAuthCookie(usuario.NombreCompleto, false);

                //  Guardar datos en sesión para usar en toda la app
                Session["UserId"] = usuario.Id;
                Session["Rol"] = usuario.Rol.Nombre;
                Session["NombreCompleto"] = usuario.NombreCompleto;
                
                // Redirigir según rol
                if (usuario.Rol.Nombre == "Maestro")
                    return RedirectToAction("Index", "Tutorias");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ViewBag.Error = "Ha ocurrido un error al iniciar sesión.";
                return View();
            }
        }

        // POST: Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon(); // limpiar todo definitivamente

            return RedirectToAction("Login", "Auth");
        }
    }
}