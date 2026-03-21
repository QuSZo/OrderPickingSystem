using Api.Dtos;

namespace Api.Orders;

public class HistoricalOrdersRepository
{
    private static readonly List<Order> _historicalOrders = new List<Order>();

    public IReadOnlyList<Order> GetAll()
    {
        return _historicalOrders.OrderByDescending(order => order.Timestamp).ToList();
    }

    public void Add(Order order)
    {
        _historicalOrders.Add(order);
    }

    public void SetFinishPickingTime(Guid? id)
    {
        Order? order = _historicalOrders.SingleOrDefault(order => order.OrderId == id);

        if (order != null)
        {
            order.FinishPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}