using System.ComponentModel.DataAnnotations;

namespace MultiPanelE_Ticaret.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // FK -> AspNetUsers.Id (string)
        public string SellerId { get; set; } = default!;
        public ApplicationUser Seller { get; set; } = default!;
    }
}
