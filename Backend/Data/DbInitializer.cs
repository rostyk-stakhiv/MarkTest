using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities;

namespace Data
{
    public static class DbInitializer
    {
        public static void SeedData(TestsDbContext context)
        {


            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role
                    {
                        RoleName = "Admin"
                    },
                    new Role
                    {
                        RoleName = "Student"
                    },
                    new Role
                    {
                        RoleName = "Teacher"
                    });
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                context.Users.Add(
                    new User
                    {
                        Name = "Ivan",
                        Surname = "Ivanov",
                        Phone = "+380961111111",
                        Email = "admin@gmail.com",
                        Login = "admin@gmail.com",
                        Password = "1B-BD-88-64-60-82-70-15-E5-D6-05-ED-44-25-22-51",
                        RoleId = context.Roles.Where(x => x.RoleName == "Admin").FirstOrDefault().Id
                    });
                context.SaveChanges();
            }
        }
    }
}
