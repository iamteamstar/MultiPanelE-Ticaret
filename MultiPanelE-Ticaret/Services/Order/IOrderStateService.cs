using MultiPanelE_Ticaret.Core.Enums;

namespace MultiPanelE_Ticaret.Services.Order
{
    public interface IOrderStateService
    {
        bool CanTransition(OrderStatus current, OrderStatus next);
    }
}
