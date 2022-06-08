using System;
using System.Collections.Generic;

namespace ITS_Middleware.Models
{
    public partial class Proyecto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string Usuario { get; set; } = null!;
        public DateTime FechaAlta { get; set; }
        public string? TipoCifrado { get; set; }
        public string? MetodoAutenticacion { get; set; }
        public string Pass { get; set; } = null!;
        public bool Activo { get; set; }
        public int? IdUsuarioRegsitra { get; set; }

        public virtual Usuario? IdUsuarioRegsitraNavigation { get; set; }
    }
}
