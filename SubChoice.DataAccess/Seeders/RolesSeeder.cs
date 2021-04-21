using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Entities;

namespace SubChoice.DataAccess.Seeders
{
    class RolesSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = new Guid("0e575c95-8003-4920-bfcc-c6803decc482"),
                    Name = Roles.Administrator,
                    NormalizedName = Roles.Administrator.ToUpperInvariant(),
                    ConcurrencyStamp = "a168fe73-bdd8-4d15-9f2f-7c38fdda54b6",
                });
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = new Guid("efcef2b3-8fae-45f1-8452-97c26292226b"),
                    Name = Roles.Teacher,
                    NormalizedName = Roles.Teacher.ToUpperInvariant(),
                    ConcurrencyStamp = "23bde7c8-43f6-47c6-8614-89c610f3f9e9",
                });
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = new Guid("986d8317-ed04-464a-921c-c3866a488566"),
                    Name = Roles.Student,
                    NormalizedName = Roles.Student.ToUpperInvariant(),
                    ConcurrencyStamp = "c7a901e8-6c4b-4a30-9cc2-9b20a7bf1c39",
                });
        }
    }
}
