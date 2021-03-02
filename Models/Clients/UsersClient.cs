using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using uul_api.Models;
using Newtonsoft.Json;

namespace uul_web.Models.Clients {
    
    public class UsersClient {
        

        private readonly HttpClient _httpClient;

        public UsersClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<UULResponse> GetUsersListAsync() {
           
            UULResponse result;
            try {
                
                using var httpResponse = await _httpClient.GetAsync("/api/webusers/list");

                httpResponse.EnsureSuccessStatusCode();

                //using var httpResponseStream = await httpResponse.Content.ReadAsStreamAsync();
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UserWebInfoDTO>>(jsonString);
                
                //var data = await _httpClient.GetFromJsonAsync<List<UserWebInfoDTO>>("/api/webusers/list");
                result = new UULResponse() { Success = true, Data = data, Message = "" };
            } catch (Exception e) {
                result = new UULResponse() { Success = false, Data = null, Message = e.Message };
            }
            return result;
        }
    }
}
