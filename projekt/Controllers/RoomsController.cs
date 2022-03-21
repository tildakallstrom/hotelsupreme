using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;
using Newtonsoft.Json;
using System.Text;

namespace projekt.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;


        // GET: Room
        public ActionResult Index()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IEnumerable<Rooms> room = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://roomapitilda.azurewebsites.net/api/Room");
                //HTTP GET
                Task<HttpResponseMessage>? responseTask = client.GetAsync("room");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Rooms>>();
                    readTask.Wait();

                    room = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    room = Enumerable.Empty<Rooms>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(room);
        }


        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rooms model)
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri("https://roomapitilda.azurewebsites.net/api/Room");
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://roomapitilda.azurewebsites.net/api/Room/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

    }

   
}