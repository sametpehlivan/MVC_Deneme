using EntityLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
          
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CreateDateTime).HasDefaultValueSql("getDate()");
            builder.Property(c => c.CreateUserName).HasDefaultValue("Undifined");
            builder.HasData(
                new Category() 
                { 
                    Id=1,
                    Name = "Telefon", 
                    Url = "telefon" 
                },
                new Category() 
                {
                    Id = 2,
                    Name = "Bilgisayar", 
                    Url = "bilgisayar" 
                },
                new Category() 
                {
                    Id = 3,
                    Name = "Elektronik", 
                    Url = "elektronik" 
                },
                new Category() 
                {
                    Id = 4,
                    Name = "Teknoloji", 
                    Url = "teknoloji" 
                },
                new Category() 
                {
                    Id = 5,
                    Name = "Beyaz Eşya", 
                    Url = "beyaz-esya" 
                }
                );
        }
    }
}
