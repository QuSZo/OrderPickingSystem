using Api.Database;
using Api.Dtos;
using Api.TravelingSalesmanAlgorithms;
using Microsoft.EntityFrameworkCore;

namespace Api.Orders;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrderPickingDbContext _dbContext;

    public OrdersRepository(OrderPickingDbContext dbContext)
    {
        _dbContext = dbContext;        
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllDtosAsync()
    {
        return await _dbContext.Orders
            .AsNoTracking()
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
            .ToListAsync();
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        return await _dbContext.Orders
            .Include(order => order.OrderedProducts)
            .Include(order => order.PickedProducts)
            .Include(order => order.TspAlgorithmResults)
                .ThenInclude(tspAlgorithmResult => tspAlgorithmResult.Path.OrderBy(p => p.OrderNumber))
            .SingleAsync(o => o.OrderId == id);
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Remove(Order order)
    {
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
    }
}