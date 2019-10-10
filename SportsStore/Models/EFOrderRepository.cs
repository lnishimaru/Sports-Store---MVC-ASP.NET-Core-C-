using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class EFOrderRepository: IOrderRepository
    {
        private ApplicationDbContext context;
        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        // retrieving the order
        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)         // join cart table   
            .ThenInclude(l => l.Product);  // join product table
        // save the order information
        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product)); // just get the product reference - don't need to save info on the product
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
