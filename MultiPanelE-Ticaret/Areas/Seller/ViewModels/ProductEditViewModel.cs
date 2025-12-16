using System.ComponentModel.DataAnnotations;
namespace MultiPanelE_Ticaret.Areas.Seller.ViewModels
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public bool IsActive { get; set; }
    }
}
