using Api.RobotOperations;

namespace Api.Orders;

public interface IOrdersRepository
{
    IReadOnlyList<Order> GetAll();
    void Add(Order order);
    Order SetStartPickingTime(Guid id, long timestamp);
    Order SetFinishPickingTime(Guid id, long timestamp);
    Order UpdateSummary(Guid id, RobotPIDSummary robotPIDSummary, double proportionalAbsoluteMean, double derivativeAbsoluteMean, double integralAbsoluteMean, double powerDifferenceAbsoluteMean);
    Order AddPickedProduct(Guid id, OrderedProduct orderedProduct);
}