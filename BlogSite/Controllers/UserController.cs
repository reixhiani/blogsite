using AutoMapper;
using BlogSite.Helpers;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Controllers
{
    public class UserController : Controller
    {

        private readonly IPostSerivce _postSerivce;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IPostSerivce postSerivce, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _postSerivce = postSerivce;
        }

        public async Task<ActionResult<IEnumerable<UserPostsViewModel>>> GetUserPosts(string userId)
        {
            try
            {
                var posts = await _postSerivce.GetPostsByUser(userId);
                var response = _mapper.Map<List<UserPostsViewModel>>(posts);

                if (!posts.Any())
                {
                    _logger.LogError(Messages.NO_DATA);
                    return View("UserPosts", response);
                }

                return View("UserPosts", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }
        }
    }
}
