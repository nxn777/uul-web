using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using uul_api.Models;

namespace uul_web.Models.Clients {
    
    public class LoginClient {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly HttpClient _httpClient;

        public LoginClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<UULResponse> LoginAsync(UserLoginInfoDTO loginInfoDTO) {
            var loginItemJson = new StringContent(
                JsonSerializer.Serialize(loginInfoDTO, _jsonSerializerOptions),
                Encoding.UTF8,
                "application/json");
            UULResponse result;
            try {
                using var httpResponse = await _httpClient.PostAsync("/api/users/login", loginItemJson);

                httpResponse.EnsureSuccessStatusCode();

                using var httpResponseStream = await httpResponse.Content.ReadAsStreamAsync();

                result = await JsonSerializer.DeserializeAsync<UULResponse>(httpResponseStream, _jsonSerializerOptions);
            } catch (Exception e) {
                result = new UULResponse() { Success = false, Data = null, Message = e.Message };
            }
            return result;
        }
    }
}
