using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public IActionResult Create(Category category)
        {
            string name = category.Name.Trim();
            if (_context.Categories.Any(c => c.Name == name))
            {
                ModelState.AddModelError(
                    nameof(category.Name),
                    "Category name already exists!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.Name = name;

                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    TempData["Success"] = "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View(category);
        }


        public IActionResult Update(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var categories = _context.Categories.Find(id);
            if(categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public IActionResult Update(Category category)
        {
            string name = category.Name.Trim();
            if (_context.Categories.Any(c => c.Name == name && c.Id != category.Id))
            {
                ModelState.AddModelError(
                    nameof(category.Name),
                    "Category name already exists!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.Name = name;

                    _context.Categories.Update(category);
                    _context.SaveChanges();
                    TempData["Update"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View(category);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categories = _context.Categories.Find(id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var categories = _context.Categories.Find(id);
            if (categories == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Remove(categories);
                    _context.SaveChanges();
                    TempData["Delete"] = "Category deleted successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View(categories);
        }
    }
}
