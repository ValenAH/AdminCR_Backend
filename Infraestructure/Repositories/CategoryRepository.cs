using Infraestructure.Database;
using Infraestructure.Database.Entities;


namespace Infraestructure.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> ListCategories();
    }
    public class CategoryRepository : ICategoryRepository
    {
        protected Context _ctx;

        public CategoryRepository(Context ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Category>> ListCategories()
        {
            return _ctx.Category.ToList();
        }

    }
}
