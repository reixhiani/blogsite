using BlogSite.Entities;
using BlogSite.Helpers;
using BlogSite.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Services
{
    public class CommentService : ICommentService
    {
        private readonly ILogger<Comment> _logger;
        private readonly IGenericRepository<Comment> _genericRepository;

        public CommentService(ILogger<Comment> logger, IGenericRepository<Comment> genericRepository)
        {
            _logger = logger;
            _genericRepository = genericRepository;
        }

        public async Task AddComment(Comment comment)
        {
            try
            {
                await _genericRepository.Add(comment);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}
