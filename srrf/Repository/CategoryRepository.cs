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
            return _context.Categories.Any(c => c.CategoryId == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
        }
        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
