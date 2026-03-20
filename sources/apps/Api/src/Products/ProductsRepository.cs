using Api.RobotOperations;

namespace Api.Products;

public class ProductsRepository
{
    private static readonly IReadOnlyList<Product> _products = Initialize();

    public IReadOnlyList<Product> GetProducts()
    {
        return _products;
    }

    private static IReadOnlyList<Product> Initialize()
    {
        return new List<Product>()
        {
            new Product() { Id = Guid.Parse("29a500d4-774b-4e35-9d6e-8b26fec5188e"), Name = "Woda", Position = new Position() { X = 0, Y = 0 } },
            new Product() { Id = Guid.Parse("ff2729e3-4535-4b80-9131-b3194af5f836"), Name = "Chleb", Position = new Position() { X = 0, Y = 1 } },
            new Product() { Id = Guid.Parse("75bc628f-f40d-40ec-bf69-3da95dab1706"), Name = "Masło", Position = new Position() { X = 0, Y = 2 } },
            new Product() { Id = Guid.Parse("e1054fc8-e84b-49e0-be59-2c9e450aabb5"), Name = "Cukier", Position = new Position() { X = 0, Y = 3 } },
            new Product() { Id = Guid.Parse("1bfdda36-61f0-43bc-bf9f-7107f9a2ba02"), Name = "Sól", Position = new Position() { X = 0, Y = 4 } },
            new Product() { Id = Guid.Parse("3294b80f-9552-42ef-a0f7-b451154ce87b"), Name = "Ryż", Position = new Position() { X = 1, Y = 0 } },
            new Product() { Id = Guid.Parse("8dee7561-de0d-43fd-bd1a-4773519ac10a"), Name = "Makaron", Position = new Position() { X = 1, Y = 1 } },
            new Product() { Id = Guid.Parse("b4495103-2422-4172-8f29-38209028f1eb"), Name = "Ziemniaki", Position = new Position() { X = 1, Y = 2 } },
            new Product() { Id = Guid.Parse("b792a9ba-51aa-476e-a476-a0341a7c7dea"), Name = "Kapusta", Position = new Position() { X = 1, Y = 3 } },
            new Product() { Id = Guid.Parse("85b82452-01f9-42e0-95c3-cf7042e6749a"), Name = "Szynka", Position = new Position() { X = 1, Y = 4 } },
            new Product() { Id = Guid.Parse("2baec646-6647-46c8-a082-be855595aa31"), Name = "Ser", Position = new Position() { X = 2, Y = 0 } },
            new Product() { Id = Guid.Parse("1160ba03-2576-43ba-a660-f1b4364bc9dd"), Name = "Twaróg", Position = new Position() { X = 2, Y = 1 } },
        };
    }
}