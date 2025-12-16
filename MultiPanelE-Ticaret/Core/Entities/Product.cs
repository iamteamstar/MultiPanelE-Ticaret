using System.ComponentModel.DataAnnotations;

namespace MultiPanelE_Ticaret.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;

        // Seller
        public string SellerId { get; set; } = null!;
        public ApplicationUser Seller { get; set; } = null!;
    }
}
