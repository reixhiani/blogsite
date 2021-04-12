using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Entities
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string ImagePath { get; set; }

        [Required]
        public string Text { get; set; }

        public ICollection<PostCategory> PostCategorys { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
