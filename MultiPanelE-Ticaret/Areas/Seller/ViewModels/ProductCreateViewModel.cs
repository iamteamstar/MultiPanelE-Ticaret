using System.ComponentModel.DataAnnotations;

namespace MultiPanelE_Ticaret.Areas.Seller.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
