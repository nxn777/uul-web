using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using uul_api.Models;
using Newtonsoft.Json;

namespace uul_web.Models.Clients {
    
    public class NewsClient {
        

        private readonly HttpClient _httpClient;

        public NewsClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<UULResponse<List<NewsWebDTO>>> GetNewsListAsync() {
           
            UULResponse<List<NewsWebDTO>> result;
            try {
                
                using var httpResponse = await _httpClient.GetAsync("/api/webnews/list");

                httpResponse.EnsureSuccessStatusCode();

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<UULResponse<List<NewsWebDTO>>>(jsonString);
            } catch (Exception e) {
                result = new UULResponse<List<NewsWebDTO>>() { Success = false, Data = null, Message = e.Message };
            }
            return result;
        }

        public async Task<UULResponse<NewsWebDTO>> GetNewsByIdAsync(long? id) {

            UULResponse<NewsWebDTO> result;
            try {

                using var httpResponse = await _httpClient.GetAsync("/api/webnews/"+id);

                httpResponse.EnsureSuccessStatusCode();

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<UULResponse<NewsWebDTO>>(jsonString);
            } catch (Exception e) {
                result = new UULResponse<NewsWebDTO>() { Success = false, Data = null, Message = e.Message };
            }
            return result;
        }

        public async Task<UULResponse<NewsWebDTO>>UpsertNewsAsync(NewsWebDTO dto) {

            UULResponse<NewsWebDTO> result;
            try {
                var data = JsonConvert.SerializeObject(dto);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                using var httpResponse = await _httpClient.PostAsync("/api/webnews/news", stringContent);

                httpResponse.EnsureSuccessStatusCode();

                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<UULResponse<NewsWebDTO>>(jsonString);
            } catch (Exception e) {
                result = new UULResponse<NewsWebDTO>() { Success = false, Data = null, Message = e.Message };
            }
            return result;
        }
    }
}
