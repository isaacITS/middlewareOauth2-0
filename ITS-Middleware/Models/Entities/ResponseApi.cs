namespace ITS_Middleware.Models.Entities
{
    public class ResponseApi
    {
        public bool Ok { get; set; }
        public string MsgHeader { get; set; }
        public int Status { get; set; }
        public string? Msg { get; set; }
    }
}
