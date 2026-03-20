using Api.Dtos;

namespace Api.Orders;

public class HistoricalOrdersRepository
{
    private static readonly List<List<OrderedProduct>> _historicalOrders = new List<List<OrderedProduct>>();

    public IReadOnlyList<IReadOnlyList<OrderedProduct>> GetAll()
    {
        return _historicalOrders;
    }

    public void Add(List<OrderedProduct> order)
    {
        _historicalOrders.Add(order);
    }
}