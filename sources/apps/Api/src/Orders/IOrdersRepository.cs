namespace Api.Orders;

public interface IOrdersRepository
{
    IReadOnlyList<Order> GetAll();
    void Add(Order order);
    void SetFinishPickingTime(Guid? id);
}