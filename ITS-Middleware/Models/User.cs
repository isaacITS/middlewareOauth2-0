using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITS_Middleware.Models
{
    [Table("Users", Schema = "dbo")]
    public partial class User
    { 
        public int Id { get; set; }

        [Display(Name = "Nombre del usaurio")]
        public string Nombre { get; set; } = null!;
        public DateTime FechaAlta { get; set; }

        [Display(Name = "Puesto del usaurio")]
        public string Puesto { get; set; } = null!;

        [Display(Name = "Email del usaurio")]
        public string? Email { get; set; }

        [Display(Name = "Password del usuario")]
        [DataType(DataType.Password)]
        public string Pass { get; set; } = null!;
    }
}
