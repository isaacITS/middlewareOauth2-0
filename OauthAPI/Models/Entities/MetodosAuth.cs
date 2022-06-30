using System;
using System.Collections.Generic;

namespace OauthAPI.Models.Entities
{
    public partial class MetodosAuth
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
