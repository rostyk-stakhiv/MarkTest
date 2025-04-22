using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IQuestionRepository:IRepository<Question>
    {
        public Task<Question> GetByIdWithDetailsAsync(int id);
    }
}
