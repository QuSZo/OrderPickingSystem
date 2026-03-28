namespace Api.Orders;

public class InMemoryOrdersRepository : IOrdersRepository
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

    public Order SetFinishPickingTime(Guid? id)
    {
        Order order = _historicalOrders.Single(order => order.OrderId == id);
        order.FinishPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        return order;
    }
}