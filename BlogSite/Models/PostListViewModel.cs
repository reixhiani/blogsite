using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Models
{
    public class PostListViewModel
    {
        public List<PostViewModel> Posts { get; set; }

        [Display(Name = "Text")]
        public string SearchText { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
