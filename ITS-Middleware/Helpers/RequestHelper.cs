using ITS_Middleware.Constants;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.Tools;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
                Timeout = TimeSpan.FromMinutes(2)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Vars.SECRET_TOKEN);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*=====> HTTP GET REQUEST <=======*/
        public async Task<List<Proyecto>> GetAllProjects()
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}Projects/GetAll");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var projects = JsonConvert.DeserializeObject<List<Proyecto>>(responseBody);
            return projects;
        }

        public async Task<Proyecto> GetProjectById(int id)
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}Projects/GetById?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<Proyecto>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        public async Task<List<Usuario>> GetAllUsers(bool getAll)
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}Users/GetAll");
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

        public async Task<Usuario> GetUserById(int id)
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}Users/GetById?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        public async Task<Usuario> GetUserByEmail(string email)
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}Users/GetByEmail?email={email}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        public async Task<List<UsuariosProyecto>> GetAllUsersByProject()
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}UsersByProject/GetAll");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<UsuariosProyecto>>(responseBody);
            return users;
        }

        public async Task<UsuariosProyecto> GetUserByProjectById(int id)
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}UsersByProject/GetById?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<UsuariosProyecto>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            return null;
        }

        public async Task<List<MetodosAuth>> GetAllAuthMethods()
        {
            var response = await httpClient.GetAsync($"{Vars.API_URI}OAuthMethod/GetAll");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var methods = JsonConvert.DeserializeObject<List<MetodosAuth>>(responseBody);
            return methods;
        }



        /* =====> HTTP POST REQUEST <======*/
        public async Task<ResponseApi> RegisterUser(Usuario userModel)
        {
            userModel.Pass = Encrypt.sha256(userModel.Pass);
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{Vars.API_URI}Users/Register", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> RegisterProject(Proyecto projectModel)
        {
            var projectJson = JsonConvert.SerializeObject(projectModel);
            var data = new StringContent(projectJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{Vars.API_URI}Projects/Register", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> RegisterUserByProject(UsuariosProyecto userModel)
        {
            userModel.Pass = Encrypt.sha256(userModel.Pass);
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{Vars.API_URI}UsersByProject/Register", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }



        /*=====> HTTP PUT REQUEST <========*/
        public async Task<ResponseApi> UpdateUser(Usuario userModel)
        {
            if(userModel.Pass == null) {
                userModel.Pass = "";
            } else {
                userModel.Pass = Encrypt.sha256(userModel.Pass);
            }
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{Vars.API_URI}Users/Update", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }
        public async Task<ResponseApi> UpdateUserStatus(int id)
        {
            var response = await httpClient.PutAsync($"{Vars.API_URI}Users/UpdateStatus?id={id}", null);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> UpdateUserTokenRecovery(string email, string token)
        {
            var dataJson = JsonConvert.SerializeObject(new { email = email, token = token });
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{Vars.API_URI}SignIn/UpdateToken", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }
        public async Task<ResponseApi> UpdateUserPassword(Usuario userModel)
        {
            userModel.Pass = Encrypt.sha256(userModel.Pass);
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{Vars.API_URI}SignIn/UpdatePassword", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> UpdateProject(Proyecto projectModel)
        {
            var projectJson = JsonConvert.SerializeObject(projectModel);
            var data = new StringContent(projectJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{Vars.API_URI}Projects/Update", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> UpdateProjectStatus(int id)
        {
            var response = await httpClient.PutAsync($"{Vars.API_URI}Projects/UpdateStatus?id={id}", null);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> UpdateUserByProject(UsuariosProyecto userModel)
        {
            if (userModel.Pass == null) {
                userModel.Pass = "";
            } else {
                userModel.Pass = Encrypt.sha256(userModel.Pass);
            }
            var userJson = JsonConvert.SerializeObject(userModel);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{Vars.API_URI}UsersByProject/Update", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> UpdateUserByProjectStatus(int id)
        {
            var response = await httpClient.PutAsync($"{Vars.API_URI}UsersByProject/UpdateStatus?id={id}", null);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }



        /*========> HTTP DELETE REQUEST <==============*/
        public async Task<ResponseApi> DeleteUser(int id)
        {
            var response = await httpClient.DeleteAsync($"{Vars.API_URI}Users/Delete?id={id}");
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> DeleteProject(int id)
        {
            var response = await httpClient.DeleteAsync($"{Vars.API_URI}Projects/Delete?id={id}");
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }

        public async Task<ResponseApi> DeleteUserByProject(int id)
        {
            var response = await httpClient.DeleteAsync($"{Vars.API_URI}UsersByProject/Delete?id={id}");
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }


        //===> UPDATE/REGISTER IMAGE FOR PROJECTS
        public async Task<ResponseApi> UploadImage(int id, string imageUrl)
        {
            var objectData = new { id = id, imageUrl = imageUrl };
            var dataJson = JsonConvert.SerializeObject(objectData);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{Vars.API_URI}Projects/UploadImage?id={id}", data);
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }
        


        /*==> SIGN IN REQUEST */
        public async Task<ResponseApi> SignIn(string email, string pass)
        {
            pass = Encrypt.sha256(pass);
            var response = await httpClient.GetAsync($"{Vars.API_URI}SignIn/SignIn?email={email}&pass={pass}");
            var resultResponse = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
            return resultResponse;
        }
    }
}
