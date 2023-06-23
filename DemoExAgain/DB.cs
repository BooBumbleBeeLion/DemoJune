using DemoExAgain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExAgain
{
    internal class DB
    {
        private readonly DemoExContext _context = new();

        public static DB Instance { get; } = new();

        private DB()
        {
            Products = _context.Products.Local.ToObservableCollection();
            Manufacturers = _context.Manufacturers.Local.ToObservableCollection();
            PickupPoints = _context.PickupPoints.ToList();
            Init();
        }

        private void Init()
        {
            _context.Manufacturers.Load();
            _context.Products.Load();          
        }

        public ObservableCollection<Product> Products { get; }
        public ObservableCollection<Manufacturer> Manufacturers { get; }
        public List<PickupPoint> PickupPoints { get; }

        public User? Authorize(string login, string password)
        {
            return CurrentUser = _context.Users.FirstOrDefault
                (u => u.Login == login && u.Password == password);
        }

        public void Exit()
        {
            CurrentUser = null;
        }

        public User? CurrentUser { get; private set; }

        public ObservableCollection<Product> ProductsInOrder { get; } = new();
        public ObservableCollection<ProductWithCount> Order { get; } = new();

        public void AddToOrder(Product p)
        {
            ArgumentNullException.ThrowIfNull(p);
            var old = Order.FirstOrDefault(pwc => pwc.Product.Id == p.Id);
            if (old != null)
            {
                old.Count++;
            }
            else
            {
                Order.Add(new ProductWithCount(p));              
            }
            ProductsInOrder.Add(p);
        }

        public void ChangeProductCount(ProductWithCount old)
        {
            ArgumentNullException.ThrowIfNull(old);
            int count = old.Count - ProductsInOrder.Where(p => p.Id == old.Product.Id).Count();
            if (count > 0)
            {
                for (int i = -1; ++i < count;)
                {
                    ProductsInOrder.Add(old.Product);
                }
            }
            else
            {
                count = -count;
                for (int i = -1; ++i < count;)
                {
                    ProductsInOrder.Remove(old.Product);
                }
            }
            if (old.Count is 0)
            {
                Order.Remove(old);
            }
        }

        public Order FormOrder(PickupPoint pp)
        {
            var order = new Order
            {
                OrderDate = DateTime.Today,
                DeliveryDate = DateTime.Today.AddDays(Order.All(p => p.Product.QuantityInStock >= 3) ? 3 : 6),
                PickupPointId = pp.Id,
                Status = "Новый",
                CodeToGet = r.Next(100, 1000)
            };
            _context.Orders.Add(order);
            _context.SaveChanges(); //сохранение изменений в бд
            foreach (var pwc in Order)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = pwc.Product.Id,
                    ProductCount = pwc.Count
                };
                _context.OrderProducts.Add(orderProduct);
            }
            _context.SaveChanges(); //очередное сохранение изменений
            Order.Clear();
            ProductsInOrder.Clear();
            return order;
        }

        private static readonly Random r = new();
    }

    class ProductWithCount
    {
        public Product Product { get; }

        public int Count { get; set; }

        public ProductWithCount(Product product, int count = 1)
        {
            Product = product;
            Count = count;

        }
    }
}
