using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Models
{
    public class AddCommentViewModel
    {
        public CommentViewModel Comment { get; set; }

        public int PostId { get; set; }

        public string UserId { get; set; }
    }
}
