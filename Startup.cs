using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using uul_web.Models.Clients;

namespace uul_web {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment _env;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
           
            services.AddHttpContextAccessor();
            AddRestClients(services, "https://192.168.100.8:5001");
            services.AddRazorPages();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options => options.LoginPath = "/Auth/login");

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        private void AddRestClients(IServiceCollection services, string baseUrl) {
            services.AddHttpClient<LoginClient>((sp, client) => {
                client.BaseAddress = new Uri("https://192.168.100.8:5001");
                client.Timeout = TimeSpan.FromSeconds(5);
            }).ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                if (_env.IsDevelopment()) {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                }
                return handler;
            });
            services.AddHttpClient<UsersClient>((sp, client) => {
                var claims = sp.GetService<IHttpContextAccessor>().HttpContext.User.Claims;
                string token = claims.Where(c => c.Type.Equals("token")).First().Value;
                client.BaseAddress = new Uri("https://192.168.100.8:5001");
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }).ConfigurePrimaryHttpMessageHandler(() => {
                var handler = new HttpClientHandler();
                if (_env.IsDevelopment()) {
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                }
                return handler;
            });
        }
    }
}
