using MultiPanelE_Ticaret.Core.Enums;

namespace MultiPanelE_Ticaret.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }

        // FK -> AspNetUsers.Id (string)
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        // FK -> AspNetUsers.Id (string) (opsiyonel)
        public string? CourierId { get; set; }
        public ApplicationUser? Courier { get; set; }

        // FK -> AspNetUsers.Id (string) (seller)
        public string SellerId { get; set; } = default!;
        public ApplicationUser Seller { get; set; } = default!;

        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
