using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Products
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}
