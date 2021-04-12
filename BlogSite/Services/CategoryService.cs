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
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _genericRepository;
        private readonly ILogger _logger;

        public CategoryService(ILogger<Category> logger, IGenericRepository<Category> genericRepository)
        {
            _logger = logger;
            _genericRepository = genericRepository;
        }

        public async Task<List<Category>> GetCategories()
        {
            try
            {
                return await _genericRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}
