namespace BaiTh.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }

        public decimal DiscountedPrice { get; set; }
        public List<ProductImage>? Images { get; set; }
        public bool IsFavorite { get; set; } // Thuộc tính để phân biệt giữa các mục trong giỏ hàng và danh sách yêu thích
    }
}
