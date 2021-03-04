using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using uul_api.Models;
using uul_web.Models.Clients;

namespace uul_web.Pages.News
{
    public class NewsEditModel : PageModel
    {
        private readonly NewsClient _client;
        private readonly ILogger<NewsEditModel> _logger;

        [BindProperty]
        public NewsWebDTO Dto { get; set; }

        public NewsEditModel(NewsClient usersClient, ILogger<NewsEditModel> logger) {
            _client = usersClient;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetNews(long? passedObject) {
            if (passedObject == null) {
                Dto = new NewsWebDTO();
                return Page();
            }

            var response = await _client.GetNewsByIdAsync(passedObject);

            if (response == null || !response.Success) {
                ModelState.AddModelError(string.Empty, "Failed to get news item");
                Dto = new NewsWebDTO();
                return Page();
            }
            Dto = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            var response = await _client.UpsertNewsAsync(Dto);
            if (response == null || !response.Success) {
                ModelState.AddModelError(string.Empty, "Failed to update news item");
                Dto = new NewsWebDTO();
                return Page();
            }
            return RedirectToPage("/News/NewsList");
        }
        public async Task<IActionResult> OnGetDelete(long? Id) {
            _logger.LogInformation("delete " + Id);
            var response = await _client.DeleteNewsByIdAsync(Id);
            if (response == null || !response.Success) {
                ModelState.AddModelError(string.Empty, "Failed to delete news item");
                Dto = new NewsWebDTO();
                return Page();
            }
            return RedirectToPage("/News/NewsList");
        }
    }
}
