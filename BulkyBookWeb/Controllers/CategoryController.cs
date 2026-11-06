using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Category category)
        {
            string name = category.Name.Trim();
            if (!await _categoryService.IsCategoryNameUniqueAsync(name) && !string.IsNullOrEmpty(name))
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

                    await _categoryService.CreateCategoryAsync(category);
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

        public async Task<IActionResult> Update(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetCategoryByIdAsync(id);
            if(categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public async Task<IActionResult> Update(Category category)
        {
            string name = category.Name.Trim();
            if (!await _categoryService.IsCategoryNameUniqueAsync(name, category.Id))
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

                    await _categoryService.UpdateCategoryAsync(category);
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetCategoryByIdAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var categories = await _categoryService.GetCategoryByIdAsync(id);
            if (categories == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.DeleteCategoryAsync(id);
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
