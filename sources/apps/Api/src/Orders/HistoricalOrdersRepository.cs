using Api.Dtos;

namespace Api.Orders;

public class HistoricalOrdersRepository
{
    private static readonly List<Order> _historicalOrders = new List<Order>();

    public IReadOnlyList<Order> GetAll()
    {
        return _historicalOrders;
    }

    public void Add(Order order)
    {
        _historicalOrders.Add(order);
    }
}