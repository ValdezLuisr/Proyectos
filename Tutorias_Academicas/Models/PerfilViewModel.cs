using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tutorias_Academicas.Models
{
    public class PerfilViewModel
    {
        public Usuario Usuario { get; set; }

        // Para alumnos
        public List<Tutoria> TutoriasInscritas { get; set; } = new List<Tutoria>();

        // Para maestros
        public List<Tutoria> TutoriasAsignadas { get; set; } = new List<Tutoria>();
    }
}