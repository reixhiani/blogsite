using BlogSite.Entities;
using BlogSite.Helpers;
using BlogSite.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Services
{
    public class PostService : IPostSerivce
    {
        private readonly IGenericRepository<Post> _genericRepository;
        private readonly ILogger _logger;

        public PostService(ILogger<Post> logger, IGenericRepository<Post> genericRepository)
        {
            _logger = logger;
            _genericRepository = genericRepository;
        }

        public async Task AddPost(Post post)
        {
            try
            {
                await _genericRepository.Add(post);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task DeletePost(Post post)
        {
            try
            {
                _genericRepository.Remove(post);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            try
            {
                return await _genericRepository.GetAllAsQueryable()
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.PostCategorys)
                        .ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<List<Post>> GetPosts()
        {
            try
            {
                return await _genericRepository.GetAllAsQueryable()
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                    .Include(p => p.PostCategorys)
                        .ThenInclude(p => p.Category)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task SaveChanges()
        {
            try
            {
                await _genericRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task UpdatePost(Post post)
        {
            try
            {
                _genericRepository.Update(post);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

        public async Task<List<Post>> GetPostsByUser(string userId)
        {
            try
            {
                return await _genericRepository.GetAllAsQueryable()
                    .Where(p => p.User.Id == userId)
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                    .Include(p => p.PostCategorys)
                        .ThenInclude(p => p.Category)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }

    }
}
