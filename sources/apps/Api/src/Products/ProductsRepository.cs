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
            new Product() { Id = Guid.Parse("29a500d4-774b-4e35-9d6e-8b26fec5188e"), Name = "Chleb", Position = new Position() { X = 0, Y = 1 } },
            new Product() { Id = Guid.Parse("ff2729e3-4535-4b80-9131-b3194af5f836"), Name = "Bułka", Position = new Position() { X = 0, Y = 2 } },
            new Product() { Id = Guid.Parse("75bc628f-f40d-40ec-bf69-3da95dab1706"), Name = "Makaron", Position = new Position() { X = 0, Y = 3 } },
            new Product() { Id = Guid.Parse("e1054fc8-e84b-49e0-be59-2c9e450aabb5"), Name = "Ryż", Position = new Position() { X = 0, Y = 4 } },
            new Product() { Id = Guid.Parse("1bfdda36-61f0-43bc-bf9f-7107f9a2ba02"), Name = "Kasza", Position = new Position() { X = 0, Y = 5 } },
            new Product() { Id = Guid.Parse("3294b80f-9552-42ef-a0f7-b451154ce87b"), Name = "Płatki owsiane", Position = new Position() { X = 0, Y = 7 } },
            new Product() { Id = Guid.Parse("8dee7561-de0d-43fd-bd1a-4773519ac10a"), Name = "Mąka", Position = new Position() { X = 0, Y = 8 } },
            new Product() { Id = Guid.Parse("b4495103-2422-4172-8f29-38209028f1eb"), Name = "Mleko", Position = new Position() { X = 0, Y = 9 } },
            new Product() { Id = Guid.Parse("b792a9ba-51aa-476e-a476-a0341a7c7dea"), Name = "Jogurt naturalny", Position = new Position() { X = 0, Y = 10 } },
            new Product() { Id = Guid.Parse("85b82452-01f9-42e0-95c3-cf7042e6749a"), Name = "Ser żółty", Position = new Position() { X = 0, Y = 11 } },
            new Product() { Id = Guid.Parse("2baec646-6647-46c8-a082-be855595aa31"), Name = "Twaróg", Position = new Position() { X = 1, Y = 1 } },
            new Product() { Id = Guid.Parse("1160ba03-2576-43ba-a660-f1b4364bc9dd"), Name = "Masło", Position = new Position() { X = 1, Y = 2 } },
            new Product() { Id = Guid.Parse("464c0a88-040f-46b2-a4b8-bfac2758878d"), Name = "Jajka", Position = new Position() { X = 1, Y = 3 } },
            new Product() { Id = Guid.Parse("d2c73dbd-0406-4b03-a8d7-1876543ee679"), Name = "Pierś z kurczaka", Position = new Position() { X = 1, Y = 4 } },
            new Product() { Id = Guid.Parse("c7be8cb8-5b4f-49d3-8096-bffda98ca285"), Name = "Szynka", Position = new Position() { X = 1, Y = 5 } },
            new Product() { Id = Guid.Parse("d68c299c-a51f-4109-81d5-da9a7a34de2c"), Name = "Kiełbasa", Position = new Position() { X = 1, Y = 7 } },
            new Product() { Id = Guid.Parse("6537614a-0456-4cfd-bf7e-779248ca21a2"), Name = "Tuńczyk w puszce", Position = new Position() { X = 1, Y = 8 } },
            new Product() { Id = Guid.Parse("f607f1c1-0b27-4d25-bc1c-95cb5bdc509b"), Name = "Ziemniaki", Position = new Position() { X = 1, Y = 9 } },
            new Product() { Id = Guid.Parse("a35777ea-7d05-49a0-bfec-322f45b1638c"), Name = "Marchewka", Position = new Position() { X = 1, Y = 10 } },
            new Product() { Id = Guid.Parse("0182b7e4-d4e6-4806-9822-4a03c076e71c"), Name = "Cebula", Position = new Position() { X = 1, Y = 11 } },
            new Product() { Id = Guid.Parse("c1fbf33d-6129-4b71-a918-4f5379d1d1a7"), Name = "Pomidor", Position = new Position() { X = 2, Y = 1 } },
            new Product() { Id = Guid.Parse("aac66119-df96-4675-98d9-680de64b575f"), Name = "Ogórek", Position = new Position() { X = 2, Y = 2 } },
            new Product() { Id = Guid.Parse("022cfe3e-de39-4182-985d-0ad737daa5bc"), Name = "Papryka", Position = new Position() { X = 2, Y = 3 } },
            new Product() { Id = Guid.Parse("f31bbaa5-c2c0-431c-a92c-6a0653c22bca"), Name = "Jabłko", Position = new Position() { X = 2, Y = 4 } },
            new Product() { Id = Guid.Parse("2b54b1fd-191f-4b9b-bcb3-6c16420ae43f"), Name = "Banan", Position = new Position() { X = 2, Y = 5 } },
            new Product() { Id = Guid.Parse("6e7ec0c4-afc8-4257-a07d-857def3e71ea"), Name = "Pomarańcza", Position = new Position() { X = 2, Y = 7 } },
            new Product() { Id = Guid.Parse("36ae9fc4-30f2-4c84-8ada-b6604a15e197"), Name = "Cukier", Position = new Position() { X = 2, Y = 8 } },
            new Product() { Id = Guid.Parse("12c23ab4-30cf-471c-9f5c-e29da30769a5"), Name = "Sól", Position = new Position() { X = 2, Y = 9 } },
            new Product() { Id = Guid.Parse("d63b4800-4bc7-46a7-b91d-f513080221cc"), Name = "Olej roślinny", Position = new Position() { X = 2, Y = 10 } },
            new Product() { Id = Guid.Parse("70987d7a-06d1-4f21-be79-390fb888baf9"), Name = "Ketchup", Position = new Position() { X = 2, Y = 11 } },
        };
    }
}




