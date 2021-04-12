using AutoMapper;
using BlogSite.Entities;
using BlogSite.Helpers;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostSerivce _postSerivce;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IPostSerivce postSerivce, IMapper mapper)
        {
            _logger = logger;
            _postSerivce = postSerivce;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<PostModel>>> GetAll()
        {
            try
            {
                var posts = await _postSerivce.GetPosts();

                if (!posts.Any())
                {
                    _logger.LogError(Messages.NO_DATA);
                    return BadRequest(new { message = string.Format(Messages.NO_DATA) });
                }

                var response = _mapper.Map<List<PostModel>>(posts);
                return View("PostLists", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        public async Task<ActionResult<PostModel>> GetById(int id)
        {
            try
            {
                var post = await _postSerivce.GetPostById(id);

                if (post == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Post id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Post with id: {id}") });
                }

                var model = _mapper.Map<PostModel>(post);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        public async Task<IActionResult> AddPost([FromBody] PostModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));
                return BadRequest(model);
            }
            try
            {
                var post = _mapper.Map<Post>(model);
                await _postSerivce.AddPost(post);
                await _postSerivce.SaveChanges();

                return Ok(new { message = Messages.CREATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        public async Task<IActionResult> UpdatePost([FromBody] PostModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(string.Format("Non valid"));
                    return BadRequest(model);
                }

                var post = await _postSerivce.GetPostById(model.Id);
                if (post == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Post id: {model.Id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Post with id: {model.Id}") });
                }

                _mapper.Map(model, post);
                await _postSerivce.UpdatePost(post);

                return Ok(new { message = Messages.UPDATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _postSerivce.GetPostById(id);

                if (post == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Post id: {id}"));
                    return BadRequest(new { message = string.Format(Messages.NOT_FOUND, $"Post with id: {id}") });
                }

                await _postSerivce.DeletePost(post);

                return Ok(new { message = string.Format(Messages.DELETED_SUCCESSFULLY, "post") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return BadRequest(new { message = Messages.UNEXPECTED_ERROR });
            }
        }

    }
}
