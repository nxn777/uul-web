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
    public class NewsListModel : PageModel
    {
        private readonly NewsClient _client;
        private readonly ILogger<NewsListModel> _logger;
        public NewsListModel(NewsClient usersClient, ILogger<NewsListModel> logger) {
            _client = usersClient;
            _logger = logger;
        }


        public ICollection<NewsWebDTO> news = new List<NewsWebDTO>();

        public async Task<IActionResult> OnGetAsync() {
            var response = await _client.GetNewsListAsync();

            if (response == null || !response.Success) {
                ModelState.AddModelError(string.Empty, "Failed to get news");
                return Page();
            }
            news = response.Data;
            return Page();
        }

        public IActionResult OnGetEdit(long? Id) {
            return RedirectToPage("/News/NewsEdit", "News", new { passedObject = Id });
        }
        public IActionResult OnPostCreate() {
            return RedirectToPage("/News/NewsEdit", "News", new { });
        }
    }
}
