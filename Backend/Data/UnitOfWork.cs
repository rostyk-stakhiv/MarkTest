using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Threading.Tasks;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TestsDbContext _context;
        private IQuestionRepository questionRepository;
        private IRepository<User> userRepository;
        private ITestRepository testRepository;
        private IRepository<Role> roleRepository;
        private IRepository<Answer> answerRepository;

        private IRepository<ThemeCount> themeCountRepository;
        public UnitOfWork(TestsDbContext context)
        {
            _context = context;
        }

        public IQuestionRepository QuestionRepository
        {
            get
            {
                if(questionRepository==null)
                {
                    questionRepository = new QuestionRepository(_context);
                }
                return questionRepository;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new Repository<User>(_context);
                }
                return userRepository;
            }
        }


        public ITestRepository TestRepository
        {
            get
            {
                if (testRepository == null)
                {
                    testRepository = new TestRepository(_context);
                }
                return testRepository;
            }
        }

        public IRepository<Role> RoleRepository
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new Repository<Role>(_context);
                }
                return roleRepository;
            }
        }

        public IRepository<Answer> AnswerRepository
        {
            get
            {
                if (answerRepository == null)
                {
                    answerRepository = new Repository<Answer>(_context);
                }
                return answerRepository;
            }
        }

        public IRepository<ThemeCount> ThemeCountRepository
        {
            get
            {
                if (themeCountRepository == null)
                {
                    themeCountRepository = new Repository<ThemeCount>(_context);
                }
                return themeCountRepository;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}