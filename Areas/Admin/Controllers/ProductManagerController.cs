using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenDaiHiep_2180605809_week_three.DataAccess;
using NguyenDaiHiep_2180605809_week_three.Models;
using NguyenDaiHiep_2180605809_week_three.Repositories;

namespace NguyenDaiHiep_2180605809_week_three.Areas.Admin.Controllers
{
	[Area("Admin")]
	/*[Authorize(Roles = "Admin")]*/
    public class ProductManagerController : Controller
    {
        //Tesst
		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;
		public ProductManagerController(IProductRepository productRepository,
								ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}
		// Hiển thị danh sách sản phẩm
		public async Task<IActionResult> IndexAdmin()
		{
			var products = await _productRepository.GetAllAsync();
			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name");
			return View(products);
		}
		// Hiển thị form thêm sản phẩm mới
		public async Task<IActionResult> AddAsync()
		{
			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name");
			return View();
		}


		// Xử lý thêm sản phẩm mới
		[HttpPost]
		public async Task<IActionResult> AddAdmin(Product product, IFormFile imageUrl)
		{
			if (ModelState.IsValid)
			{
				// Lưu hình ảnh đại diện
				product.ImageUrl = await SaveImage(imageUrl);
				await _productRepository.AddAsync(product);
				return RedirectToAction(nameof(Index));
			}
			// Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name");
			return View(product);
		}

		private async Task<string> SaveImage(IFormFile image)
		{
			var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
			using (var fileStream = new FileStream(savePath, FileMode.Create))
			{
				await image.CopyToAsync(fileStream);
			}
			return "/images/" + image.FileName; // Trả về đường dẫn tương đối
		}


		// Hiển thị thông tin chi tiết sản phẩm
		public async Task<IActionResult> DisplayAdmin(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}
		// Hiển thị form cập nhật sản phẩm
		public async Task<IActionResult> UpdateAdmin(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name",
			product.CategoryId);
			return View(product);
		}
		// Xử lý cập nhật sản phẩm
		[HttpPost]
		public async Task<IActionResult> UpdateAdmin(int id, Product product, IFormFile imageUrl)
		{
			if (id != product.Id)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				product.ImageUrl = await SaveImage(imageUrl);
				await _productRepository.UpdateAsync(product);
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}
		// Hiển thị form xác nhận xóa sản phẩm
		public async Task<IActionResult> DeleteAdmin(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}
		// Xử lý xóa sản phẩm
		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmedAdmin(int id)
		{
			await _productRepository.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}

		/*private readonly ApplicationDbContext _context;

        public ProductManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> IndexAdmin()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult CreateAdmin()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("Id,Name,Price,Description,ImageUrl,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> EditAdmin(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(int id, [Bind("Id,Name,Price,Description,ImageUrl,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> DeleteAdmin(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAdmin(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }*/
	}
}
