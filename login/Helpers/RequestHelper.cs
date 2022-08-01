using login.Constants;
using login.Models.Entities;
using Newtonsoft.Json;

namespace login.Helpers
{
    public class RequestHelper
    {
        private readonly HttpClient httpClient = new();
        private static readonly HttpClientHandler clientHandler = new();

        public RequestHelper()
        {
            httpClient = new HttpClient(clientHandler, false)
            {
                BaseAddress = new Uri(Vars.API_URI),
                Timeout = TimeSpan.FromSeconds(60)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<ResponseApi> SignIn(string email, string pass, string phoneNumber)
        {
            if (string.IsNullOrEmpty(pass))
            {
                var res = await httpClient.GetAsync($"{Vars.API_URI}SignIn/SignInService?email={email}&phoneNumber={phoneNumber}");
                var resBody = JsonConvert.DeserializeObject<ResponseApi>(res.Content.ReadAsStringAsync().Result);
                return resBody;
            }
            pass = Encrypt.sha256(pass);
            var response = await httpClient.GetAsync($"{Vars.API_URI}SignIn/SignInUserProject?email={email}&pass={pass}");
            var responseBody = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return responseBody;
        }


        public async Task<Proyecto> GetProjectByName(string project)
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}Projects/GetByName?project={project}");
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var projectRes = JsonConvert.DeserializeObject<Proyecto>(responseBody);
                return projectRes;
        }

        public async Task<List<MetodosAuth>> GetAllAuthMethods()
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}OAuthMethod/GetAll");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var methods = JsonConvert.DeserializeObject<List<MetodosAuth>>(responseBody);
            return methods;
        }

    }
}
