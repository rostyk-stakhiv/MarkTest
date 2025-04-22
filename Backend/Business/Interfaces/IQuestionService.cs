using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IQuestionService : ICrud<QuestionModel>
    {
        IEnumerable<QuestionModel> GetAllWithParams(string s, string sort_by, string sort_type, int offset, int limit);
        Task CopyAsync(QuestionModel orderModel);
        IEnumerable<QuestionModel> GetAllByOwnerId(int id);
        IEnumerable<QuestionModel> GetAllByOwnerIdWithParams(int id, string s, string sort_by, string sort_type, int offset, int limit);
        IEnumerable<string> GetSubjects(int id);
        IEnumerable<string> GetThemesForSubject(int id, string subject);
    }
}
