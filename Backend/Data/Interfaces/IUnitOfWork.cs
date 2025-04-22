using Data.Entities;
using System;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IQuestionRepository QuestionRepository { get; }
        
        IRepository<User> UserRepository { get; }
        
        ITestRepository TestRepository { get; }
        
        IRepository<Role> RoleRepository { get; }
        
        IRepository<Answer> AnswerRepository { get; }

        IRepository<ThemeCount> ThemeCountRepository { get; }

        Task<int> SaveAsync();
    }
}