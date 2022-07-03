﻿using OauthAPI.Models.Entities;
using System;
using System.Collections.Generic;

namespace OauthAPI.Models.Entities
{
    public partial class Proyecto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public string MetodosAutenticacion { get; set; } = null!;
        public bool Activo { get; set; }
        public string Enlace { get; set; }
        public int? IdUsuarioRegsitra { get; set; }
    }
}
