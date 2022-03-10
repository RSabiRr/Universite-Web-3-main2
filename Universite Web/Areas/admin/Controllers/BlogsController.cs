﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Universite_Web.Data;
using Universite_Web.Models;

namespace Universite_Web.Areas.admin.Controllers
{
    [Area("admin")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BlogsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: admin/Blogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.ToListAsync());
        }

        // GET: admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: admin/Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (ModelState.IsValid)

                if (ModelState.IsValid)
                {
                    if (blog.ImageFile != null)
                    {
                        if (blog.ImageFile.ContentType == "image/jpeg" || blog.ImageFile.ContentType == "image/png")
                        {
                            if (blog.ImageFile.Length <= 3000000)
                            {
                                string FileName = Guid.NewGuid() + "-" + blog.ImageFile.FileName;
                                string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", FileName);
                                using (var stream = new FileStream(FilePath, FileMode.Create))
                                {
                                    blog.ImageFile.CopyTo(stream);
                                }
                                blog.Image = FileName;
                                _context.Add(blog);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));

                            }
                            else
                            {
                                ModelState.AddModelError("", "you can choose only 3 mb image file");
                                return View(blog);
                            }


                        }
                        else
                        {
                            ModelState.AddModelError("", "you can choose only image file");
                            return View(blog);

                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", " choose image file");
                        return View(blog);

                    }
                
            }
            return View(blog);
        }

        // GET: admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (blog.ImageFile != null)
                    {
                        if (blog.ImageFile.ContentType == "image/jpeg" || blog.ImageFile.ContentType == "image/png")
                        {
                            if (blog.ImageFile.Length <= 3000000)
                            {
                                string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", blog.Image);
                                if (System.IO.File.Exists(olddata))
                                {
                                    System.IO.File.Delete(olddata);
                                }
                                string FileName = Guid.NewGuid() + "-" + blog.ImageFile.FileName;
                                string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", FileName);
                                using (var stream = new FileStream(FilePath, FileMode.Create))
                                {
                                    blog.ImageFile.CopyTo(stream);
                                }
                                blog.Image = FileName;

                                _context.Blogs.Update(blog);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));

                            }
                            else
                            {
                                ModelState.AddModelError("", "you can choose only 3 mb image file");
                                return View(blog);
                            }


                        }
                        else
                        {
                            ModelState.AddModelError("", "you can choose only image file");
                            return View(blog);

                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", " choose image file");
                        return View(blog);

                    }
                }
            }
            return View(blog);
        }

        // GET: admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", blog.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
