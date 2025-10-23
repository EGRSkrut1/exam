using System;
using System.Linq;
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        var orders = new List<Order>
        {
            new Order { Id = 1, CustomerName = "Валерий", TotalAmount = 150000, OrderDate = new DateTime(2025, 10, 25) },
            new Order { Id = 2, CustomerName = "Егор", TotalAmount = 230000, OrderDate = new DateTime(2025, 11, 12) },
        };

        var orderItems = new List<OrderItem>
        {
            new OrderItem { Id = 1, OrderId = 1, ProductName = "МАК БУК", Quantity = 1, Price = 100000 },
            new OrderItem { Id = 2, OrderId = 2, ProductName = "Айфон", Quantity = 1, Price = 49900 },
            new OrderItem { Id = 3, OrderId = 2, ProductName = "Дошырик", Quantity = 1, Price = 15000 },
            new OrderItem { Id = 4, OrderId = 3, ProductName = "Аниме девочка плюшевая", Quantity = 1, Price = 1800 },
        };

        foreach (var order in orders)
        {
            order.OrderItems = orderItems.Where(oidi => oidi.OrderId == order.Id).ToList();
        }

        var result = await Task.Run(() =>
        {
            var filterDate = new DateTime(2025, 11, 1);

            return orders
                .Where(order => order.OrderDate > filterDate)
                .Select(order => new
                {
                    OrderId = order.Id,
                    CustomerName = order.CustomerName,
                    OrderDate = order.OrderDate,
                    Items = order.OrderItems,
                    TotalOrderSum = order.OrderItems.Sum(item => item.Price * item.Quantity)
                })
                .ToList();
        });


        Console.WriteLine("Заказы после 1 ноября 2025:");
        foreach (var order in result)
        {
            Console.WriteLine($"\nЗаказ #{order.OrderId}");
            Console.WriteLine($"Клиент: {order.CustomerName}");
            Console.WriteLine($"Дата: {order.OrderDate:dd.MM.yyyy}");
            Console.WriteLine($"Общая сумма: {order.TotalOrderSum} руб.");
            Console.WriteLine("Товары:");

            foreach (var item in order.Items)
            {
                Console.WriteLine($"{item.Quantity} x {item.Price} руб.");
            }
        }
    }


}