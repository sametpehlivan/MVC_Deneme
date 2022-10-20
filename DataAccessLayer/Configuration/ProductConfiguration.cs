using EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            
            builder.Property(p => p.CreateDateTime).HasDefaultValueSql("getDate()");
            
            builder.Property(p => p.CreateUserName).HasDefaultValue("Undifined");
            builder.HasData(
                new Product() 
                { 
                    Id = 1,
                    Name = "Samsung S6", 
                    Url = "samsung-s6", 
                    Description = "iyi", 
                    ImageUrl = "1", 
                    Stars = 4.1, 
                    Price = 3000, 
                    UserVote = 9, 
                    StockQuantity = 100 
                },
                new Product() 
                {
                    Id = 2,
                    Name = "Samsung S7", 
                    Url = "samsung-s6", 
                    Description = "güzel", 
                    ImageUrl = "2", 
                    Stars = 3, 
                    Price = 2500, 
                    UserVote = 7, 
                    StockQuantity = 100 
                },
                new Product() 
                {
                    Id = 3,
                    Name = "Samsung S8", 
                    Url = "samsung-s6", 
                    Description = "good", 
                    ImageUrl = "3", 
                    Stars = 3.0, 
                    Price = 2000, 
                    UserVote = 3, 
                    StockQuantity = 1 
                },
                new Product() 
                {
                    Id = 4,
                    Name = "iPhone X", 
                    Url = "iphone-x", 
                    Description = "on numara", 
                    ImageUrl = "4", 
                    Stars = 4.5, 
                    Price = 4000, 
                    UserVote = 2, 
                    StockQuantity = 100 
                },
                new Product() 
                {
                    Id = 5,
                    Name = "iPhone 8", 
                    Url = "iphone-8", 
                    Description = "alınır", 
                    ImageUrl = "5", 
                    Stars = 3.9, 
                    Price = 3800, 
                    UserVote = 1, 
                    StockQuantity = 100 
                },
                new Product() {
                    Id = 6,
                    Name = "iPhone 7", 
                    Url = "iphone-7", 
                    Description = "on numara", 
                    ImageUrl = "6", 
                    Stars = 3.7, 
                    Price = 3000, 
                    UserVote = 5, 
                    StockQuantity = 0 
                }
                );

        }
    }
}
