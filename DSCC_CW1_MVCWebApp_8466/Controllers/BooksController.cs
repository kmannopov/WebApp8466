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
using System.Net.Http.Json;

namespace DSCC_CW1_MVCWebApp_8466.Controllers
{
    public class BooksController : Controller
    {
        private readonly DSCC_CW1_MVCWebApp_8466Context _context;
        private string Baseurl = "http://ec2-3-120-150-207.eu-central-1.compute.amazonaws.com:5000/";

        public BooksController(DSCC_CW1_MVCWebApp_8466Context context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            //Hosted web API REST Service base url

            List<Book> ProdInfo = new List<Book>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new
               MediaTypeWithQualityHeaderValue("application/json"));
                 HttpResponseMessage Res = await client.GetAsync("api/Book");
            if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                     ProdInfo = JsonConvert.DeserializeObject<List<Book>>(PrResponse);
                }
                //returning the Product list to view
                return View(ProdInfo);
            }
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> CreateAsync()
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
            }
            ViewData["BookGenreId"] = new SelectList(ProdInfo, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Author,Isbn,BookGenreId")] Book book)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                using (var client = new HttpClient())
                {
                    var randomNumber = new Random();
                    book.Id = randomNumber.Next(1000);
                    client.BaseAddress = new Uri(Baseurl);
                    var postTask = await client.PostAsJsonAsync<Book>("api/Book", book);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
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
            }
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
            ViewData["BookGenreId"] = new SelectList(ProdInfo, "Id", "Name", book.BookGenreId);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,Isbn,BookGenreId")] Book _book)
        {
            if (id != _book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        HttpResponseMessage Res = await client.GetAsync("api/Book/" + id);
                        Book book = null;
                        //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api
                            var PrResponse = Res.Content.ReadAsStringAsync().Result;
                            //Deserializing the response recieved from web api and storing into the Product list
                            book = JsonConvert.DeserializeObject<Book>(PrResponse);
                        }
                        //HTTP POST
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(_book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(_book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book genre = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Book/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var BookResponse = Res.Content.ReadAsStringAsync().Result;
                    genre = JsonConvert.DeserializeObject<Book>(BookResponse);
                }
            }
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.DeleteAsync("api/Book/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
