using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ITestRepository:IRepository<Test>
    {
        Task<Test> GetByIdWithDetailsAsync(int id);
        List<Test> GetAllWithDetails();
    }
}
