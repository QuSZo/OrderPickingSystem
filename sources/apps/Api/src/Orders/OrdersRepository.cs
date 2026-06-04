using Api.Database;
using Api.RobotOperations;

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

    public Order SetStartPickingTime(Guid id)
    {
        Order order = _dbContext.Orders.Single(order => order.OrderId == id);
        order.StartPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        _dbContext.SaveChanges();

        return order;
    }

    public Order SetFinishPickingTime(Guid id)
    {
        Order order = _dbContext.Orders.Single(order => order.OrderId == id);
        order.FinishPickingTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        _dbContext.SaveChanges();

        return order;
    }

    public Order AddPickedProduct(Guid id, OrderedProduct orderedProduct)
    {
        Order order = _dbContext.Orders.Single(order => order.OrderId == id);
        order.PickedProducts.Add(orderedProduct);

        _dbContext.SaveChanges();

        return order;
    }

    public Order UpdateSummary(Guid id, RobotPIDSummary robotPIDSummary, double proportionalAbsoluteMean, double derivativeAbsoluteMean, double integralAbsoluteMean, double powerDifferenceAbsoluteMean)
    {
        Order order = _dbContext.Orders.Single(order => order.OrderId == id);
        order.ProportionalHistory = robotPIDSummary.ProportionalHistory;
        order.DerivativeHistory = robotPIDSummary.DerivativeHistory;
        order.IntegralHistory = robotPIDSummary.IntegralHistory;
        order.PowerDifferenceHistory = robotPIDSummary.PowerDifferenceHistory;
        order.ProportionalAbsoluteMean = proportionalAbsoluteMean;
        order.DerivativeAbsoluteMean = derivativeAbsoluteMean;
        order.IntegralAbsoluteMean = integralAbsoluteMean;
        order.PowerDifferenceAbsoluteMean = powerDifferenceAbsoluteMean;

        _dbContext.SaveChanges();
        
        return order;
    }
}