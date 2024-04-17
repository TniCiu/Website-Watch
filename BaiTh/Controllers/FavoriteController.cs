using System.Linq;
using BaiTh.Data;
using BaiTh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BaiTh.Repository;

namespace BaiTh.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        public FavoriteController(ApplicationDbContext dbContext, IProductRepository
       productRepository)
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
        }

        // Action để hiển thị danh sách yêu thích của người dùng
        public IActionResult Index()
        {
            // Lấy tất cả các mục được đánh dấu là yêu thích từ cơ sở dữ liệu
            var favoriteItems = _dbContext.CartItems.Where(item => item.IsFavorite).ToList();

            return View(favoriteItems);
        }

        // Action để thêm hoặc xóa sản phẩm khỏi danh sách yêu thích
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            // Tìm kiếm sản phẩm trong danh sách yêu thích của người dùng
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(item => item.ProductId == productId);

            if (cartItem != null)
            {
                // Đảo ngược trạng thái yêu thích của sản phẩm
                cartItem.IsFavorite = !cartItem.IsFavorite;
                await _dbContext.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        // Action để thêm sản phẩm vào danh sách yêu thích
        public async Task<IActionResult> AddToFavorites(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product != null)
            {
                // Kiểm tra xem sản phẩm đã tồn tại trong danh sách yêu thích chưa
                var existingFavorite = await _dbContext.CartItems.FirstOrDefaultAsync(item => item.ProductId == productId && item.IsFavorite);

                if (existingFavorite != null)
                {
                    // Nếu sản phẩm chưa tồn tại trong danh sách yêu thích, thêm nó vào
                    var favoriteItem = new CartItem
                    {
                        ProductId = productId,
                        Name = product.Name,
                        Price = product.Price,
                        Quantity = 1, // Số lượng mặc định khi thêm vào danh sách yêu thích
                        ImageUrl = product.ImageUrl,
                        IsFavorite = true // Đánh dấu sản phẩm là yêu thích
                    };

                    _dbContext.CartItems.Add(favoriteItem);
                    await _dbContext.SaveChangesAsync();

                    // Hiển thị thông báo cho người dùng
                    TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào danh sách yêu thích.";
                }
                else
                {
                    // Hiển thị thông báo cho người dùng nếu sản phẩm đã tồn tại trong danh sách yêu thích
                    TempData["InfoMessage"] = "Sản phẩm đã có trong danh sách yêu thích của bạn.";
                }

                // Quay trở lại trang sản phẩm hoặc trang danh sách yêu thích
                return RedirectToAction("Index", "Product");
            }
            else
            {
                // Hiển thị thông báo lỗi nếu sản phẩm không tồn tại
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return NotFound();
            }
        }

    }
}