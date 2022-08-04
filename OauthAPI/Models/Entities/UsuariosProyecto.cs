using System;
using System.Collections.Generic;

namespace OauthAPI.Models.Entities
{
    public partial class UsuariosProyecto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Pass { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaAcceso { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        public string? TokenRecovery { get; set; }
        public int? IdProyecto { get; set; }
    }
}
