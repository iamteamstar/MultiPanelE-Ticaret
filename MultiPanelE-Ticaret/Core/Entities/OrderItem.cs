namespace MultiPanelE_Ticaret.Core.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public string SellerId { get; set; } = default!;
        public ApplicationUser Seller { get; set; } = default!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }


}
