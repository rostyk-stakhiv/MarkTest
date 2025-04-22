using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseClass
    {
        protected readonly TestsDbContext _context;
        private DbSet<T> _enteties;
        public Repository(TestsDbContext context)
        {
            _context = context;
            _enteties = context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {

            if (entity == null) throw new ArgumentNullException("Null entity");

           await _enteties.AddAsync(entity);

        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("Not Found");

            _enteties.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _enteties.SingleOrDefaultAsync(s => s.Id == id);
            Delete(entity);
        }

        public IQueryable<T> FindAll()
        {
            return _enteties;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _enteties.SingleOrDefaultAsync(s => s.Id == id); 

            return entity;
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("Null entity");
            if (!_enteties.Any(x => x.Id == entity.Id)) throw new ArgumentNullException("Not Found");
            _context.Update(entity);
        }
    }
}
