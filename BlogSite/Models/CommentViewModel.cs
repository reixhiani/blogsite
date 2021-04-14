using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }
        public PostViewModel Post { get; set; }

        public string UserId { get; set; }
        public ApplicationUserViewModel User { get; set; }
    }
}
