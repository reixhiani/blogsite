using BlogSite.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSite.Helpers
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                    new Category
                    {
                        Id = 1,
                        Name = "Technology"
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Sport"
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "Music"
                    }
                );
        }
    }
}
