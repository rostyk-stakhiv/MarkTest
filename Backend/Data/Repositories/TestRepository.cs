using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TestRepository : Repository<Test>, ITestRepository
    {
        public TestRepository(TestsDbContext context) : base(context)
        {
        }
        public async Task<Test> GetByIdWithDetailsAsync(int id)
        {
            var test =await  GetByIdAsync(id);
            test.Owner = _context.Users.FirstOrDefault(x => x.Id == test.OwnerId);
            test.User = _context.Users.FirstOrDefault(x => x.Id == test.UserId);
            test.ThemeCount = _context.ThemeCounts.Where(x=>x.TestId == id).ToList();
            return test;
        }

        public List<Test> GetAllWithDetails()
        {
            var result = FindAll().ToList();
            foreach (var test in result)
            {
                test.Owner = _context.Users.FirstOrDefault(x => x.Id == test.OwnerId);
                test.User = _context.Users.FirstOrDefault(x => x.Id == test.UserId);
                test.ThemeCount = _context.ThemeCounts.Where(x => x.TestId == test.Id).ToList();
            }
            return result;
        }
    }
}
