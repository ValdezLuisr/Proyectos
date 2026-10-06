using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Tutorias_Academicas.Models;

namespace Tutorias_Academicas.Data
{
    public class TutoriasAcademicasContext : DbContext
    {

        public TutoriasAcademicasContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Tutoria> Tutorias { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; } 

    }
}