using login.Constants;
using login.Models.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
                BaseAddress = new Uri(VarsHelpers.API_URI),
                Timeout = TimeSpan.FromMinutes(2)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<ResponseApi> SignIn(SigninData signinData)
        {
            try
            {
                if (!string.IsNullOrEmpty(signinData.Pass)) signinData.Pass = Encrypt.Sha256(signinData.Pass);

                var dataJson = JsonConvert.SerializeObject(signinData);
                var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{VarsHelpers.API_URI}SignIn/SignInUserProject", data);
                var responseBody = JsonConvert.DeserializeObject<ResponseApi>(response.Content.ReadAsStringAsync().Result);
                return responseBody;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<Proyecto> GetProjectByName(string project)
        {
            try
            {
                var response = await httpClient.GetAsync($"{VarsHelpers.API_URI}Projects/GetByName?project={project}");
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var projectRes = JsonConvert.DeserializeObject<Proyecto>(responseBody);
                return projectRes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseApi> UpdateToken(string token, string email, string Url)
        {
            try
            {
                var response = await httpClient.PutAsync($"{VarsHelpers.API_URI}UsersByProject/UpdateToken?token={token}&email={email}&Url={Url}", null);
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var userResult = JsonConvert.DeserializeObject<ResponseApi>(responseBody);
                return userResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseApi> UpdatePasword(UpdateData updateData)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", updateData.TokenJwt);

                updateData.NewPass = Encrypt.Sha256(updateData.NewPass);
                var dataJson = JsonConvert.SerializeObject(updateData);
                var data = new StringContent(dataJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"{VarsHelpers.API_URI}UsersByProject/UpdatePassword", data);
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var userResult = JsonConvert.DeserializeObject<ResponseApi>(responseBody);
                return userResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
