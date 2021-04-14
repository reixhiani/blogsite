using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Models
{
    public class UserPostsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
