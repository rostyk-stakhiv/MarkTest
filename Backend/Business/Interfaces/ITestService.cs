using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITestService:ICrud<TestModel>
    {
        Task<TestModel> GenerateTestAsync(int testId, int difficultyLevel);
        Task<double> CheckTestAsync(TestModel test);
        Task EnrollToTest(int userId, int testId);

        Task<List<TestModel>> GetAllOwnerTests(int ownerId, string subject);
        Task<List<TestModel>> CheckTestsAsync(int id, string name, DateTime fromDate, DateTime toDate);
        Task<List<string>> GetTestsNames(int id);
        Task<List<string>> GetSubjects(int id);
        Task SendEmailsAsync(int id, string receivers, string link);
        Task<List<TestModel>> GetAllUserTests(int id);
    }
}
