﻿using ITS_Middleware.Models.Entities;
using System;
using System.Collections.Generic;

namespace ITS_Middleware.Models.Entities
{
    public partial class Proyecto
    {
        public Proyecto()
        {
            UsuariosProyectos = new HashSet<UsuariosProyecto>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public string MetodosAutenticacion { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool Activo { get; set; }
        public string Enlace { get; set; }
        public int? IdUsuarioRegsitra { get; set; }

        public virtual Usuario? IdUsuarioRegsitraNavigation { get; set; }
        public virtual ICollection<UsuariosProyecto> UsuariosProyectos { get; set; }
    }
}
