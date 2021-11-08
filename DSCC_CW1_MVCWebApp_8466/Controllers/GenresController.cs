using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DSCC_CW1_MVCWebApp_8466.Data;
using DSCC_CW1_MVCWebApp_8466.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DSCC_CW1_MVCWebApp_8466.Controllers
{
    public class GenresController : Controller
    {
        private readonly DSCC_CW1_MVCWebApp_8466Context _context;
        private string Baseurl = "http://ec2-3-120-150-207.eu-central-1.compute.amazonaws.com:5000/";

        public GenresController(DSCC_CW1_MVCWebApp_8466Context context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            List<Genre> ProdInfo = new List<Genre>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Genre");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    ProdInfo = JsonConvert.DeserializeObject<List<Genre>>(PrResponse);
                }
                //returning the Product list to view
                return View(ProdInfo);
            }
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Genre genre = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Genre/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var BookResponse = Res.Content.ReadAsStringAsync().Result;
                    genre = JsonConvert.DeserializeObject<Genre>(BookResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }                   
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                using (var client = new HttpClient())
                {
                    var randomNumber = new Random();
                    genre.Id = randomNumber.Next(150);
                    client.BaseAddress = new Uri(Baseurl);
                    var postTask = await client.PostAsJsonAsync<Genre>("api/Genre", genre);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Genre genre = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Genre/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var BookResponse = Res.Content.ReadAsStringAsync().Result;
                    genre = JsonConvert.DeserializeObject<Genre>(BookResponse);
                }
            }
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Genre _genre)
        {
            if (id != _genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        HttpResponseMessage Res = await client.GetAsync("api/Genre/" + id);
                        Genre genre = null;
                        //Checking the response is successful or not which is sent using HttpClient
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api
                            var PrResponse = Res.Content.ReadAsStringAsync().Result;
                            //Deserializing the response recieved from web api and storing into the Product list
                            genre = JsonConvert.DeserializeObject<Genre>(PrResponse);
                        }
                        //HTTP POST
                        var postTask = client.PutAsJsonAsync<Genre>("api/Genre/" + _genre.Id, _genre);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(_genre.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Genre genre = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Genre/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var BookResponse = Res.Content.ReadAsStringAsync().Result;
                    genre = JsonConvert.DeserializeObject<Genre>(BookResponse);
                }
            }
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.DeleteAsync("api/Genre/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Message = "This genre is currently in use. Please delete the associated books first.";
            return View();
        }

        private bool GenreExists(int id)
        {
            return _context.Genre.Any(e => e.Id == id);
        }
    }
}
