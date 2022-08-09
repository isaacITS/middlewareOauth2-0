using System;
using System.Collections.Generic;

namespace login.Models.Entities
{
    public partial class UpdateData
    {
        public string? Token { get; set; }
        public string? TokenJwt { get; set; }
        public string? Email { get; set; }
        public string? NewPass { get; set; }
        public string? ProjectName { get; set; }
        public string? ImageProject { get; set; }
    }
}
