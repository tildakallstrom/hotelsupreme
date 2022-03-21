#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projekt.Data;
using projekt.Models;
using System.Drawing;
using LazZiya.ImageResize;

namespace projekt.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,ImageFile,Author")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                if(posts.ImageFile != null)
                {
                    //upload file
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(posts.ImageFile.FileName); //image filename
                    string extension = Path.GetExtension(posts.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yyyyMMddssff") + extension;

                    //add filename to model
                    posts.ImageName = fileName;

                    //output path
                    string path = Path.Combine(wwwRootPath + "wwwroot/images/" + fileName);

                    //move to folder
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await posts.ImageFile.CopyToAsync(fileStream);
                    }

                    //resize image
                    CreateImageFiles(fileName);

                }
                _context.Add(posts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(posts);
        }
        //resize images
        private void CreateImageFiles(string fileName)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            //create thumbnail
            using(var img = Image.FromFile(Path.Combine(wwwRootPath + "wwwroot/images", fileName)))
            {
                img.Scale(800, 600).SaveAs(Path.Combine(wwwRootPath + "wwwroot/images/thumb_" + fileName));

            }
        }

        // GET: Posts/Edit/5
        
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }


            var posts = await _context.Posts.FindAsync(Id);
            if (posts == null)
            {
                return NotFound();
            }
            return View(posts);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

      /*  public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Text,ImageFile,Author")] Posts posts, IFormFile Image)
        {
            if (id != posts.Id)
            {
                return NotFound();
            }
            if (Image != null)
            {
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);

                //Get url To Save
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", ImageName);

                posts.ImagePath = "img/" + ImageName;

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsExists(posts.Id))
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
            return View(posts);
        }
        */
        public async Task<IActionResult> Edit(int? Id, [Bind("Id,Title,Text,ImageName,Author")] Posts posts)
        {
            if (Id != posts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsExists(posts.Id))
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
           
            return View(posts);
        } 

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posts = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(posts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostsExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
