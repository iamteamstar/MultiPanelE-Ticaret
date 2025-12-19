namespace MultiPanelE_Ticaret.Core.DTOs
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // AspNetUsers.Id -> string
        public string SellerId { get; set; } = default!;
    }
}
