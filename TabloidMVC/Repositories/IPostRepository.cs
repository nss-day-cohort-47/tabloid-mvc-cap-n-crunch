using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void DeletePost(int id);
        void UpdatePost(Post post);
        List<Post> GetAllPublishedPosts();
        List<Post> GetAllPublishedPostsByUser(int id);
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        
    }
}