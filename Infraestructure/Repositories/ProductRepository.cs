using Infraestructure.Database;
using Infraestructure.Database.Entities;
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
            return _ctx.Product.ToList();
        }
        public async Task<Product> GetProductById(int productId)
        {
            return _ctx.Product.Where(x => x.ProductId == productId).FirstOrDefault();
        }

        public async Task<bool> SaveProduct(Product product)
        {
            _ctx.Product.Add(product);
            return _ctx.SaveChanges()>0;
        }
    }
}
