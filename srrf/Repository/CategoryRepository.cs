using srrf.Data;
using srrf.Interface;
using srrf.Models;

namespace srrf.Repository
{
    public class CategoryRepository : ICategory
    {
        private readonly SrrfContext _context;
        public CategoryRepository(SrrfContext context)
        {
            _context = context;
        }

        public bool CategoriesExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }
        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public Category GetCategoryByName(string name)
        {
            return _context.Categories.Where(c => c.Name == name).FirstOrDefault();
        }
    }
}
