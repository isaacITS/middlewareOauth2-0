namespace login.Models.Entities
{
    public class ResponseApi
    {
        public bool Ok { get; set; }
        public int Status { get; set; }
        public string? MsgHeader { get; set; }
        public string? Msg { get; set; }
        public string? Token { get; set; }
        public string? TokenJwt { get; set; }
    }
}
