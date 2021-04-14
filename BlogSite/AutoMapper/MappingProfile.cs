using AutoMapper;
using BlogSite.Entities;
using BlogSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostViewModel>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.PostCategorys.Select(y => y.Category).ToList()))
                .ReverseMap()
                .ForMember(d => d.PostCategorys, opt => opt.MapFrom(s => s.Categories.Select
                (c => new PostCategory { PostId = s.Id, CategoryId = c.Id })))
                .ForMember(d => d.User, opt => opt.Ignore());

            CreateMap<Post, UserPostsViewModel>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.PostCategorys.Select(y => y.Category).ToList()));

            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
        }
    }
}
