using BlogSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Interfaces
{
    public interface IPostSerivce
    {
        IQueryable<Post> GetPosts();

        Task<Post> GetPostById(int id);

        Task AddPost(Post post);

        Task UpdatePost(Post post);

        Task DeletePost(Post post);

        Task SaveChanges();

        Task<List<Post>> GetPostsByUser(string userId);
    }
}
