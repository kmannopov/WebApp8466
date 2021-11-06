using DSCC_CW1_MVCWebApp_8466.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DSCC_CW1_MVCWebApp_8466.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

            public async Task<ActionResult> Index()
            {
                string Baseurl = "https://localhost:44362/";
                List<Book> BookInfo = new List<Book>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.GetAsync("api/Book");
                    if (Res.IsSuccessStatusCode)
                    {
                        BookInfo = await client.GetFromJsonAsync<List<Book>>("api/Book");
                    }

                    return View(BookInfo);
                }
            }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            string Baseurl = "https://localhost:44362/";
            Book book = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Book/" + id);
            if (Res.IsSuccessStatusCode)
                {
                    var BookResponse = Res.Content.ReadAsStringAsync().Result;
                     book = JsonConvert.DeserializeObject<Book>(BookResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(book);
        }
        // POST: Product/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Book _book)
        {
            try
            {
                // TODO: Add update logic here
                string Baseurl = "http://localhost:44362/";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage Res = await client.GetAsync("api/Book/" + id);
                    Book book = null;

                if (Res.IsSuccessStatusCode)
                    {
                        var BookResponse = Res.Content.ReadAsStringAsync().Result;
                        book = JsonConvert.DeserializeObject<Book>(BookResponse);
                    }
                    _book.BookGenre = book.BookGenre;
                    var postTask = client.PutAsJsonAsync<Book>("api/Book/" + _book.Id, _book);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }

            return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
