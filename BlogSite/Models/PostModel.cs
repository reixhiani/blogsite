using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public string Text { get; set; }

        public ICollection<CategoryModel> Categories { get; set; }

        public ICollection<CommentModel> Comments { get; set; }

        public ApplicationUserModel User { get; set; }
    }
}
