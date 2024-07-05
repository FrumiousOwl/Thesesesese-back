using srrf.Models;

namespace srrf.Interface
{
    public interface ICategory
    {
        ICollection <Category> GetCategories ();
        Category GetCategory (int id);
        Category GetCategoryByName (string name);
        bool CategoriesExists (int categoryId);
    }
}
