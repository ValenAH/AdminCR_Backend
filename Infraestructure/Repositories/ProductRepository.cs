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
        Task<Product> GetProductById(int IdProduct);
    }
    public class ProductRepository :IProductRepository
    {
        protected Context _ctx;

        public ProductRepository(Context ctx)
        {
            _ctx = ctx;
        }
        public async Task<Product> GetProductById(int idProduct)
        {
            return _ctx.Product.Where(x => x.IdProduct == idProduct).FirstOrDefault();
        }
    }
}
