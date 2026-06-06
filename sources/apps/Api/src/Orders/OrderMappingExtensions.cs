using Api.Dtos;

namespace Api.Orders;

public static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto
        {
            OrderId = order.OrderId,
            OrderedProducts = order.OrderedProducts,
            PickedProducts = order.PickedProducts,
            TspAlgorithm = order.TspAlgorithm,
            Timestamp = order.Timestamp,
            Distance = order.Distance,
            StartPickingTime = order.StartPickingTime,
            FinishPickingTime = order.FinishPickingTime,
            ProportionalAbsoluteMean = order.ProportionalAbsoluteMean,
            DerivativeAbsoluteMean = order.DerivativeAbsoluteMean,
            IntegralAbsoluteMean = order.IntegralAbsoluteMean,
            PowerDifferenceAbsoluteMean = order.PowerDifferenceAbsoluteMean,
        };
    }
}