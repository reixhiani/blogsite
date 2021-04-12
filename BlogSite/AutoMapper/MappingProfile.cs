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
            CreateMap<Post, PostModel>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(x => x.PostCategorys.Select(y => y.Category).ToList()))
                .ReverseMap()
                .ForMember(d => d.PostCategorys, opt => opt.MapFrom(s => s.Categories.Select
                (c => new PostCategory { PostId = s.Id, CategoryId = c.Id })))
                .ForMember(d => d.User, opt => opt.Ignore());

            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Comment, CommentModel>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserModel>().ReverseMap();
        }
    }
}
