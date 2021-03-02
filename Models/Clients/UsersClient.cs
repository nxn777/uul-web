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

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UserWebInfoDTO>>(jsonString);
                result = new UULResponse() { Success = true, Data = data, Message = "" };
            } catch (Exception e) {
                result = new UULResponse() { Success = false, Data = null, Message = e.Message };
            }
            return result;
        }

        public async Task<UULResponse>UpdateUsertAsync(UserWebInfoDTO dto) {

            UULResponse result;
            try {
                var data = JsonConvert.SerializeObject(dto);
                var stringContent = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
                using var httpResponse = await _httpClient.PostAsync("/api/webusers/update", stringContent);

                httpResponse.EnsureSuccessStatusCode();

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<UserWebInfoDTO>(jsonString);

                result = new UULResponse() { Success = true, Data = output, Message = "" };
            } catch (Exception e) {
                result = new UULResponse() { Success = false, Data = null, Message = e.Message };
            }
            return result;
        }
    }
}
