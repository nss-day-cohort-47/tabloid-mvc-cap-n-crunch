using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        void DeleteCategory(int id);
        Category GetCategoryById(int id);
        List<Category> GetAll();


    }
}