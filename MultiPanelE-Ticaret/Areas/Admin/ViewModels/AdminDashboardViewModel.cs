namespace MultiPanelE_Ticaret.Areas.Admin.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ActiveCouriers { get; set; }

        public List<RecentOrderVm> RecentOrders { get; set; } = new();
    
}
}
