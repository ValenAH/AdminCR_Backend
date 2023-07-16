using Infraestructure.Database;
using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> ListProducts();
        Task<Product> GetProductById(int productId);
        Task<bool> SaveProduct(Product product);
        Task<bool> UpdateProduct(Product product);
    }
    public class ProductRepository :IProductRepository
    {
        protected Context _ctx;

        public ProductRepository(Context ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Product>> ListProducts()
        {
            return _ctx.Product.Include("Category").ToList();
        }
        public async Task<Product> GetProductById(int productId)
        {
            return _ctx.Product.Include("Category").Where(x => x.Id == productId).FirstOrDefault();
        }

        public async Task<bool> SaveProduct(Product product)
        {
            _ctx.Product.Add(product);
            return _ctx.SaveChanges()>0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            _ctx.Product.Update(product);
            return _ctx.SaveChanges()>0;
        }
    }
}
