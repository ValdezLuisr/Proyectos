using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Tutorias_Academicas.Models
{
    // ==== ENTIDADES ====

    [Table("Roles")]          
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    [Table("Usuarios")]       
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string ContrasenaHash { get; set; }

        public int RolId { get; set; }
        public virtual Rol Rol { get; set; }
    }

    [Table("Tutorias")]
    public class Tutoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Materia es obligatorio.")]
        public string Materia { get; set; }

        [Required(ErrorMessage = "Debe ingresar una descripción.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe ingresar un horario.")]
        public string Horario { get; set; }

        [Range(1, 1000, ErrorMessage = "El cupo debe ser mayor a 0.")]
        public int Cupo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la modalidad.")]
        public string Modalidad { get; set; }

        [Required(ErrorMessage = "Debe ingresar el aula.")]
        public string Aula { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un maestro.")]
        public int MaestroId { get; set; }
        public virtual Usuario Maestro { get; set; }
        public virtual ICollection<Inscripcion> Inscripciones { get; set; }
    }

    [Table("Inscripciones")]  //  usa la tabla Inscripciones
    public class Inscripcion
    {
        public int Id { get; set; }

        public int TutoriaId { get; set; }
        public virtual Tutoria Tutoria { get; set; }

        public int AlumnoId { get; set; }
        public virtual Usuario Alumno { get; set; }
    }

    
   
}