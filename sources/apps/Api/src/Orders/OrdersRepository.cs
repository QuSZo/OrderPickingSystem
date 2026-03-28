using Api.Database;

namespace Api.Orders;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrderPickingDbContext _dbContext;

    public OrdersRepository(OrderPickingDbContext dbContext)
    {
        _dbContext = dbContext;        
    }

    public IReadOnlyList<Order> GetAll()
    {
        return _dbContext.Orders.OrderByDescending(order => order.Timestamp).ToList();
    }

    public void Add(Order order)
    {
        _dbContext.Orders.Add(order);
        _dbContext.SaveChanges();
    }

    public void SetFinishPickingTime(Guid? id)
    {
        Order? order = _dbContext.Orders.SingleOrDefault(order => order.OrderId == id);

        if (order != null)
        {
            order.FinishPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        _dbContext.SaveChanges();
    }
}