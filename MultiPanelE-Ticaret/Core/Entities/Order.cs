using MultiPanelE_Ticaret.Core.Enums;

namespace MultiPanelE_Ticaret.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public string SellerId { get; set; } = default!;
        public ApplicationUser Seller { get; set; } = default!;

        public string? CourierId { get; set; }
        public ApplicationUser? Courier { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Created;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

}
