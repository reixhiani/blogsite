using AutoMapper;
using BlogSite.Entities;
using BlogSite.Helpers;
using BlogSite.Interfaces;
using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostSerivce _postSerivce;
        private readonly ICategoryService _categoryService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IPostSerivce postSerivce, IMapper mapper,
            ICategoryService categoryService,
            IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _mapper = mapper;
            _postSerivce = postSerivce;
            _categoryService = categoryService;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetAll()
        {
            try
            {
                var posts = await _postSerivce.GetPosts();
                var response = _mapper.Map<List<PostViewModel>>(posts);

                if (!posts.Any())
                {
                    _logger.LogError(Messages.NO_DATA);
                    return View("PostLists", response);
                }

                return View("PostLists", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult<PostViewModel>> GetById(int id)
        {
            try
            {
                var post = await _postSerivce.GetPostById(id);

                if (post == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Post id: {id}"));
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                var model = _mapper.Map<PostViewModel>(post);

                return View("PostDetails", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<PostViewModel>> AddPost()
        {

            var model = new PostViewModel();
            var categories = await _categoryService.GetCategories();
            model.Categories = _mapper.Map<List<CategoryViewModel>>(categories);
            return View("AddPost", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromForm] PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));

                var categories = await _categoryService.GetCategories();
                model.Categories = _mapper.Map<List<CategoryViewModel>>(categories);
                return View(model);
            }
            try
            {
                model.Categories = new List<CategoryViewModel>();
                foreach (var id in model.CategoryIds)
                {
                    var categoryModel = new CategoryViewModel
                    {
                        Id = id
                    };
                    model.Categories.Add(categoryModel);
                }

                var post = _mapper.Map<Post>(model);
                post.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                string fileName = "defaultImage.jpg";
                if (model.Image != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                    var extension = Path.GetExtension(model.Image.FileName);
                    if (!allowedExtensions.Contains(extension))
                    {
                        var categories = await _categoryService.GetCategories();
                        model.Categories = _mapper.Map<List<CategoryViewModel>>(categories);
                        ModelState.AddModelError("Image", "File type not allowed.");
                        return View(model);
                    }
                    else
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                        post.ImagePath = fileName;
                    }
                }

                await _postSerivce.AddPost(post);
                await _postSerivce.SaveChanges();

                return RedirectToAction("GetAll", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdatePost(int id)
        {
            try
            {
                var post = await _postSerivce.GetPostById(id);
                if (post == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Post id: {id}"));
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                // Prevent current user from modifing other users posts from url
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userPosts = await _postSerivce.GetPostsByUser(userId);

                if (!userPosts.Contains(post))
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                var model = _mapper.Map<PostViewModel>(post);

                // set selected categories
                model.CategoryIds = model.Categories.Select(c => c.Id).ToList();

                // get all categories
                var allCategories = await _categoryService.GetCategories();
                model.Categories = _mapper.Map<List<CategoryViewModel>>(allCategories);

                return View("UpdatePost", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePost([FromForm] PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Non valid"));

                var categories = await _categoryService.GetCategories();
                model.Categories = _mapper.Map<List<CategoryViewModel>>(categories);
                return View(model);
            }
            try
            {
                // get post from DB
                var post = await _postSerivce.GetPostById(model.Id);
                if (post == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Post id: {model.Id}"));
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                model.Categories = new List<CategoryViewModel>();
                foreach (var id in model.CategoryIds)
                {
                    var categoryModel = new CategoryViewModel
                    {
                        Id = id
                    };
                    model.Categories.Add(categoryModel);
                }
         
                _mapper.Map(model, post);
                post.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                string fileName = null;
                if (model.Image != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                    var extension = Path.GetExtension(model.Image.FileName);
                    if (!allowedExtensions.Contains(extension))
                    {
                        var categories = await _categoryService.GetCategories();
                        model.Categories = _mapper.Map<List<CategoryViewModel>>(categories);
                        ModelState.AddModelError("Image", "File type not allowed.");
                        return View(model);
                    }
                    else
                    {
                        // if post has image delete it and update it with new one
                        if (model.ImagePath != null)
                        {
                            string existingFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ImagePath);
                            System.IO.File.Delete(existingFilePath);
                        }

                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }
                        post.ImagePath = fileName;
                    }
                }

                await _postSerivce.UpdatePost(post);
                return RedirectToAction("GetAll", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _postSerivce.GetPostById(id);
                if (post == null)
                {
                    _logger.LogError(string.Format(Messages.NOT_FOUND, $"Post id: {id}"));
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                // Prevent current user from modifing other users posts
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userPosts = await _postSerivce.GetPostsByUser(userId);

                if (!userPosts.Contains(post))
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                // if post has image delete it
                if (post.ImagePath != null)
                {
                    string existingFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", post.ImagePath);
                    System.IO.File.Delete(existingFilePath);
                }

                await _postSerivce.DeletePost(post);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                return RedirectToAction("Error", "Error");
            }
        }

    }
}
