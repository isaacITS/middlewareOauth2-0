using login.Constants;
using login.Models.Entities;
using Newtonsoft.Json;
using System.Text;

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


        public async Task<ResponseApi> SignIn(SigninData signinData)
        {
            if (!string.IsNullOrEmpty(signinData.Pass)) signinData.Pass = Encrypt.sha256(signinData.Pass);

            var dataJson = JsonConvert.SerializeObject(signinData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{Vars.API_URI}SignIn/SignInUserProject", data);
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

        public async Task<UsuariosProyecto> GetUserProject(string email)
        {
            var emailObj = new { email = email };
            var userJson = JsonConvert.SerializeObject(emailObj);

            var data = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{Vars.API_URI}UsersByProject/GetByEmail", data);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var userResult = JsonConvert.DeserializeObject<UsuariosProyecto>(responseBody);
            return userResult;
        }

        public async Task<UsuariosProyecto> GetUserProjectById(int id)
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}UsersByProject/GetById");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var userResult = JsonConvert.DeserializeObject<UsuariosProyecto>(responseBody);
            return userResult;
        }


        public async Task<ResponseApi> UpdateUserProject(UsuariosProyecto userModel)
        {
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{Vars.API_URI}UsersByProject/Update", data);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var userResult = JsonConvert.DeserializeObject<ResponseApi>(responseBody);
            return userResult;
        }

    }
}
