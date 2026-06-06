using Api.Dtos;

namespace Api.Orders;

public class InMemoryOrdersRepository : IOrdersRepository
{
    private static readonly List<Order> _historicalOrders = new List<Order>();

    public Task<IReadOnlyList<OrderDto>> GetAllDtosAsync()
    {
        IReadOnlyList<OrderDto> result = _historicalOrders
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderedProducts = o.OrderedProducts,
                PickedProducts = o.PickedProducts,
                TspAlgorithm = o.TspAlgorithm,
                Timestamp = o.Timestamp,
                Distance = o.Distance,
                StartPickingTime = o.StartPickingTime,
                FinishPickingTime = o.FinishPickingTime,
                ProportionalAbsoluteMean = o.ProportionalAbsoluteMean,
                DerivativeAbsoluteMean = o.DerivativeAbsoluteMean,
                IntegralAbsoluteMean = o.IntegralAbsoluteMean,
                PowerDifferenceAbsoluteMean = o.PowerDifferenceAbsoluteMean,
            })
            .OrderByDescending(order => order.Timestamp)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<Order> GetByIdAsync(Guid id)
    {
        Order order = _historicalOrders
            .Single(o => o.OrderId == id);

        return Task.FromResult(order);
    }

    public Task AddAsync(Order order)
    {
        _historicalOrders.Add(order);
        return Task.CompletedTask;
    }

    public Task Update(Order order)
    {
        var index = _historicalOrders.FindIndex(o => o.OrderId == order.OrderId);

        if (index != -1)
        {
            _historicalOrders[index] = order;
        }

        return Task.CompletedTask;
    }

    public Task Remove(Order order)
    {
        _historicalOrders.Remove(order);
        return Task.CompletedTask;
    }
}