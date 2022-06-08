using System;
using System.Collections.Generic;

namespace ITS_Middleware.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Proyectos = new HashSet<Proyecto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaAlta { get; set; }
        public string Puesto { get; set; } = null!;
        public string? Email { get; set; }
        public string Pass { get; set; } = null!;
        public bool Activo { get; set; }

        public virtual ICollection<Proyecto> Proyectos { get; set; }
    }
}
