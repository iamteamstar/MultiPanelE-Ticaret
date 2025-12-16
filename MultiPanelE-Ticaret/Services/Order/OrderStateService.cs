using MultiPanelE_Ticaret.Core.Enums;

namespace MultiPanelE_Ticaret.Services.Order
{
    public class OrderStateService : IOrderStateService
    {
        private static readonly Dictionary<OrderStatus, OrderStatus[]> _rules =
            new()
            {
                { OrderStatus.Created, new[] { OrderStatus.Preparing } },
                { OrderStatus.Preparing, new[] { OrderStatus.OnTheWay } },
                { OrderStatus.OnTheWay, new[] { OrderStatus.Delivered } },
                { OrderStatus.Delivered, Array.Empty<OrderStatus>() }
            };

        public bool CanTransition(OrderStatus current, OrderStatus next)
        {
            return _rules.ContainsKey(current) && _rules[current].Contains(next);
        }
    }
}
