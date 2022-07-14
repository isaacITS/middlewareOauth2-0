using ITS_Middleware.Constants;
using ITS_Middleware.Models.Entities;
using Newtonsoft.Json;
using System.Text;

namespace ITS_Middleware.Helpers
{
    public class RequestHelper
    {
        private readonly HttpClient httpClient;
        private static readonly HttpClientHandler clientHandler = new();

        public RequestHelper()
        {
            httpClient = new HttpClient(clientHandler, false)
            {
                BaseAddress = new Uri(Vars.API_URI),
                Timeout = TimeSpan.FromSeconds(59.9)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*=====> HTTP GET REQUEST <=======*/
        public dynamic GetAllProjects()
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}Projects/GetAll").Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var projects = JsonConvert.DeserializeObject<List<Proyecto>>(responseBody);
            return projects;
        }

        public dynamic GetProjectById(int id)
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}Projects/GetById?id={id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<Proyecto>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        /*
        public async Task<List<Proyecto>> GetAllProjects()
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{Vars.API_URI}Projects/GetAll");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var projects = JsonConvert.DeserializeObject<List<Proyecto>>(responseBody);
            return projects;
        }*/

        public dynamic GetAllUsers(bool getAll)
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}Users/GetAll").Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<Usuario>>(responseBody);
            if (getAll)
            {
                return users;
            }
            var userRemove = users.SingleOrDefault(uR => uR.Id == 1);
            users.Remove(userRemove);
            return users;
        }

        public dynamic GetUserById(int id)
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}Users/GetById?id={id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        public dynamic GetUserByEmail(string email)
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}Users/GetByEmail?email={email}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        public dynamic GetAllUsersByProject()
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}UsersByProject/GetAll").Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<UsuariosProyecto>>(responseBody);
            return users;
        }

        public dynamic GetUserByProjectById(int id)
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}UsersByProject/GetById?id={id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<UsuariosProyecto>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        public dynamic GetAllAuthMethods()
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}OAuthMethod/GetAll").Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var methods = JsonConvert.DeserializeObject<List<MetodosAuth>>(responseBody);
            return methods;
        }



        /* =====> HTTP POST REQUEST <======*/
        public dynamic RegisterUser(Usuario userModel)
        {
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync($"{Vars.API_URI}Users/Register", data).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic RegisterProject(Proyecto projectModel)
        {
            var projectJson = JsonConvert.SerializeObject(projectModel);
            var data = new StringContent(projectJson, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync($"{Vars.API_URI}Projects/Register", data).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic RegisterUserByProject(UsuariosProyecto userModel)
        {
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync($"{Vars.API_URI}UsersByProject/Register", data).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }



        /*=====> HTTP PUT REQUEST <========*/
        public dynamic UpdateUser(Usuario userModel)
        {
            if(userModel.Pass == null) {
                userModel.Pass = "";
            }
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync($"{Vars.API_URI}Users/Update", data).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }
        public dynamic UpdateUserStatus(int id)
        {
            var response = httpClient.PutAsync($"{Vars.API_URI}Users/UpdateStatus?id={id}", null).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic UpdateUserTokenRecovery(string email, string token)
        {
            var response = httpClient.PutAsync($"{Vars.API_URI}SignIn/UpdateToken?email={email}&token={token}", null).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }
        public dynamic UpdateUserPassword(Usuario userModel)
        {
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync($"{Vars.API_URI}SignIn/UpdatePassword", data).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic UpdateProject(Proyecto projectModel)
        {
            var projectJson = JsonConvert.SerializeObject(projectModel);
            var data = new StringContent(projectJson, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync($"{Vars.API_URI}Projects/Update", data).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic UpdateProjectStatus(int id)
        {
            var response = httpClient.PutAsync($"{Vars.API_URI}Projects/UpdateStatus?id={id}", null).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic UpdateUserByProject(UsuariosProyecto userModel)
        {
            if (userModel.Pass == null)
            {
                userModel.Pass = "";
            }
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = httpClient.PutAsync($"{Vars.API_URI}UsersByProject/Update", data).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic UpdateUserByProjectStatus(int id)
        {
            var response = httpClient.PutAsync($"{Vars.API_URI}UsersByProject/UpdateStatus?id={id}", null).Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }



        /*========> HTTP DELETE REQUEST <==============*/
        public dynamic DeleteUser(int id)
        {
            var response = httpClient.DeleteAsync($"{Vars.API_URI}Users/Delete?id={id}").Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic DeleteProject(int id)
        {
            var response = httpClient.DeleteAsync($"{Vars.API_URI}Projects/Delete?id={id}").Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public dynamic DeleteUserByProject(int id)
        {
            var response = httpClient.DeleteAsync($"{Vars.API_URI}UsersByProject/Delete?id={id}").Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }


        //===> UPDATE PASSWORD PROCESS
        


        /*==> SIGN IN REQUEST */
        public dynamic SignIn(string email, string pass)
        {
            var response = httpClient.GetAsync($"{Vars.API_URI}SignIn/SignIn?email={email}&pass={pass}").Result;
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }
    }
}
