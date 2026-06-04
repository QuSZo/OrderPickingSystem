using Api.RobotOperations;

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

    public Order SetStartPickingTime(Guid id)
    {
        Order order = _historicalOrders.Single(order => order.OrderId == id);
        order.StartPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        return order;
    }

    public Order SetFinishPickingTime(Guid id)
    {
        Order order = _historicalOrders.Single(order => order.OrderId == id);
        order.FinishPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        return order;
    }

    public Order AddPickedProduct(Guid id, OrderedProduct orderedProduct)
    {
        Order order = _historicalOrders.Single(order => order.OrderId == id);
        order.PickedProducts.Add(orderedProduct);

        return order;
    }

    public Order UpdateSummary(Guid id, RobotPIDSummary robotPIDSummary, double proportionalAbsoluteMean, double derivativeAbsoluteMean, double integralAbsoluteMean, double powerDifferenceAbsoluteMean)
    {
        Order order = _historicalOrders.Single(order => order.OrderId == id);
        order.ProportionalHistory = robotPIDSummary.ProportionalHistory;
        order.DerivativeHistory = robotPIDSummary.DerivativeHistory;
        order.IntegralHistory = robotPIDSummary.IntegralHistory;
        order.PowerDifferenceHistory = robotPIDSummary.PowerDifferenceHistory;
        order.ProportionalAbsoluteMean = proportionalAbsoluteMean;
        order.DerivativeAbsoluteMean = derivativeAbsoluteMean;
        order.IntegralAbsoluteMean = integralAbsoluteMean;
        order.PowerDifferenceAbsoluteMean = powerDifferenceAbsoluteMean;

        return order;
    }
}