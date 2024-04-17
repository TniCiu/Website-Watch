using Microsoft.AspNetCore.Mvc;
using BaiTh.Data;
using BaiTh.Models;

namespace BaiTh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CheckoutApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            // Thực hiện logic thanh toán ở đây
            // Ví dụ: Lưu đơn hàng vào cơ sở dữ liệu
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return Ok(new { orderId = order.Id, message = "Thanh toán thành công" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, new { message = "Lỗi khi thực hiện thanh toán", error = ex.Message });
            }
        }
    }
}
