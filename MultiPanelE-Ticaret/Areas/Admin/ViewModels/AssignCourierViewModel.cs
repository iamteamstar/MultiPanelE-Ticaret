using System.ComponentModel.DataAnnotations;

namespace MultiPanelE_Ticaret.Areas.Admin.ViewModels
{
    public class AssignCourierViewModel
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string CourierId { get; set; } = "";

        public List<CourierOptionVm> Couriers { get; set; } = new();
    }

    public class CourierOptionVm
    {
        public string Id { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
