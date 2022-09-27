using GameStore.Models;
using GameStore.Models.Pages;

namespace GameStore.Interfaces
{
    public interface ICategory
    {
        PagedList<Category> GetCategories(QueryOptions options);
        IEnumerable<Category> GetAllCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }

}
