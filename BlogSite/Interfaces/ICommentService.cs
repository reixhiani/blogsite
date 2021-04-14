using BlogSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Interfaces
{
    public interface ICommentService
    {
        Task AddComment(Comment comment);
    }
}
