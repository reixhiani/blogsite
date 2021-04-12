using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ImagePath { get; set; }

        public IFormFile Image { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Category")]
        public List<int> CategoryIds { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public ApplicationUserViewModel User { get; set; }

    }
}
