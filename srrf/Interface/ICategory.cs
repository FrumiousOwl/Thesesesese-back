using srrf.Models;

namespace srrf.Interface
{
    public interface ICategory
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        bool CategoriesExists(int categoryId);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool CreateCategory(Category category);
        bool Save();
    }
}
