using System;
using System.Collections.Generic;

namespace OauthAPI.Models.Entities
{
    public partial class SigninData
    {
        public string? Email { get; set; }
        public string? Pass { get; set; }
        public string? PhoneNumber { get; set; }
        public int? ProjectId { get; set; }
        public string? UserUid { get; set; }
    }
}
