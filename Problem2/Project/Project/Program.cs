using System;
using System.Collections.Generic;
using System.Linq;

namespace SieMarket
{
   
    public class OrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderItem(string name, int quantity, decimal price)
        {
            ProductName = name;
            Quantity = quantity;
            UnitPrice = price;
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Order(int id, string customerName)
        {
            OrderId = id;
            CustomerName = customerName;
        }
    }

    public class StoreManager
    {
        
        public decimal CalculateOrderTotal(Order order)
        {
            decimal subtotal = order.Items.Sum(i => i.Quantity * i.UnitPrice);

            if (subtotal > 500m)
            {
                return subtotal * 0.9m;
            }
            return subtotal;
        }

        
        public string GetTopSpender(List<Order> orders)
        {
            if (orders == null || !orders.Any()) return "No customers found";

            return orders
                .GroupBy(o => o.CustomerName)
                .Select(g => new
                {
                    Name = g.Key,
                    TotalSpent = g.Sum(o => CalculateOrderTotal(o))
                })
                .OrderByDescending(x => x.TotalSpent)
                .FirstOrDefault()?.Name;
        }

        
        public Dictionary<string, int> GetPopularProducts(List<Order> orders)
        {
            return orders
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ProductName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(i => i.Quantity)
                );
        }
    }

    
    class Program
    {
        static void Main()
        {
            var manager = new StoreManager();

            var order1 = new Order(1, "Alice");
            order1.Items.Add(new OrderItem("Laptop", 1, 1200m)); 

            var order2 = new Order(2, "Bob");
            order2.Items.Add(new OrderItem("Mouse", 2, 25m));
            order2.Items.Add(new OrderItem("Keyboard", 1, 80m));

            var orders = new List<Order> { order1, order2 };

            Console.WriteLine($"Top Spender: {manager.GetTopSpender(orders)}");
        }
    }
}