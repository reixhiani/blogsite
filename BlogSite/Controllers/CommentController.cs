using AutoMapper;
using BlogSite.Entities;
using BlogSite.Helpers;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogSite.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostSerivce _postService;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ICommentService commentService, IPostSerivce postService,
            IMapper mapper, ILogger<CommentController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _postService = postService;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AddComment()
        {
            return RedirectToAction("GetAll", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment([FromForm] CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));
                return RedirectToAction("GetById", "Home", new { id = model.PostId});
            }
            try
            {
                var comment = _mapper.Map<Comment>(model);

                var post = await _postService.GetPostById(model.PostId);
                var user = await _userManager.FindByIdAsync(model.UserId);

                comment.User = user;
                comment.Post = post;
                comment.CreatedAt = DateTime.Now;

                await _commentService.AddComment(comment);
                return RedirectToAction("GetById", "Home", new { id = model.PostId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }

        }
    }
}
