using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class QuestionRepository : Repository<Question>,IQuestionRepository
    {
        public QuestionRepository(TestsDbContext context) : base(context)
        {
        }
        public async Task<Question> GetByIdWithDetailsAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            entity.Owner = _context.Users.FirstOrDefault(x => x.Id == entity.OwnerId);
            return entity;
        }
    }
}
