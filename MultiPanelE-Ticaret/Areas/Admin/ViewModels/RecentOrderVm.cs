namespace MultiPanelE_Ticaret.Areas.Admin.ViewModels
{
    public class RecentOrderVm
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
