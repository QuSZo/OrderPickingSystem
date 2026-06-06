using Api.Dtos;

namespace Api.Orders;

public interface IOrdersRepository
{
    Task<IReadOnlyList<OrderDto>> GetAllDtosAsync();
    Task<Order> GetByIdAsync(Guid id);
    Task AddAsync(Order order);
    Task Update(Order order);
    Task Remove(Order order);
}