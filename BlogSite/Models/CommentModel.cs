﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public PostModel Post { get; set; }
    }
}
