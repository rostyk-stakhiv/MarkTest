using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Data;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Random _random;
        public TestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _random = new Random();
        }

        public async Task AddAsync(TestModel model)
        {
            var test = _mapper.Map<Test>(model);
            await _unitOfWork.TestRepository.AddAsync(test);
            await _unitOfWork.SaveAsync();
        }

        public async Task<double> CheckTestAsync(TestModel test)
        {
            test.User = null;
            test.Owner=null;
            double totalMark = 0;
            double mark = 0;
            foreach (var question in test.Questions)
            {
                question.Owner = null;
                var answer = _unitOfWork.AnswerRepository.FindAll().FirstOrDefault(x => x.QuestionId == question.Id);
                if (answer == null)
                {
                    mark += question.Mark;
                }
                else
                {
                    if (question.Answers.Count > 0)
                    {
                        if (question.Type == QuestionType.single)
                        {
                            if (question.Answers[0] == answer.Answers[0])
                            {
                                mark += question.Mark;
                            }

                        }
                        else if (question.Type == QuestionType.wordfree)
                        {

                            if (answer.Answers.Contains(question.Answers[0]))
                            {
                                mark += question.Mark;
                            }
                        }
                        else
                        {
                            var partMark = question.Mark / answer.Answers.Count();
                            foreach (var item in question.Answers)
                            {
                                if (answer.Answers.Contains(item))
                                {
                                    mark += partMark;
                                }
                                else
                                {
                                    mark -= partMark;
                                }
                            }
                        }
                    }
                }
                totalMark += question.Mark;
            }
            if(test.MaxMark * (mark / totalMark) * (0.4 + 0.2 * test.DificultyLevel)>test.Mark)
            {
                test.Mark = test.MaxMark * (mark / totalMark) * (0.4 + 0.2 * test.DificultyLevel);
            }
            test.Questions = null;
            await UpdateAsync(test);
            return test.MaxMark * (mark / totalMark) * (0.4 + 0.2 * test.DificultyLevel);
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            try
            {
                await _unitOfWork.TestRepository.DeleteByIdAsync(modelId);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TestModel> GenerateTestAsync(int testId, int difficultyLevel)
        {
            var test = await _unitOfWork.TestRepository.GetByIdWithDetailsAsync(testId);
            if(test.StartDate>DateTime.Now)
            {
                throw new MarkTestException("Тест ще не почався");
            }
            if(test.EndDate<DateTime.Now||test.NumberOfAttempts<1)
            {
                throw new MarkTestException("Тест вже не доступний для вас");
            }
            test.NumberOfAttempts -= 1;
            _unitOfWork.TestRepository.Update(test);
            await _unitOfWork.SaveAsync();
            test.DificultyLevel = difficultyLevel;
            var questions = _unitOfWork.QuestionRepository.FindAll().Where(x => x.OwnerId == test.OwnerId
            && x.Subject == test.Subject && x.DificultyLevel <= difficultyLevel).ToList();
            test.Questions = new List<Question>();
            var tempThemeCount = new Dictionary<string, int>();
            foreach (var themeCount in test.ThemeCount)
            {
                if (tempThemeCount.ContainsKey(themeCount.Theme))
                {
                    tempThemeCount[themeCount.Theme] += themeCount.Count;
                }
                else
                {
                    tempThemeCount.Add(themeCount.Theme, themeCount.Count);
                }
            }
            foreach (var themecount in tempThemeCount)
            {
                var themeQuestions = questions.Where(x => x.Theme == themecount.Key).ToList();
                for (int i = 0; i < themecount.Value ; i++)
                {
                    var k = _random.Next(themeQuestions.Count);
                    test.Questions.Add(themeQuestions[k]);
                    themeQuestions.RemoveAt(k);
                }
            }
            
            return _mapper.Map<TestModel>(test);
        }

        public IEnumerable<TestModel> GetAll()
        {
            return _mapper.Map<List<TestModel>>(_unitOfWork.TestRepository.FindAll());
        }

        public IEnumerable<TestModel> GetAllForUser(int userId)
        {
            var result = _unitOfWork.TestRepository.FindAll().Where(x => x.UserId == userId);
            return _mapper.Map<List<TestModel>>(result);
        }


        public IEnumerable<TestModel> GetAllForOwner(int ownerId)
        {
            var result = _unitOfWork.TestRepository.GetAllWithDetails().Where(x => x.OwnerId == ownerId);
            return _mapper.Map<List<TestModel>>(result);
        }

        public async Task<TestModel> GetByIdAsync(int id)
        {
            var test = await _unitOfWork.TestRepository.GetByIdAsync(id);
            if (test == null)
            {
                throw new ArgumentNullException();
            }

            return _mapper.Map<TestModel>(test);
        }

        public async Task UpdateAsync(TestModel model)
        {

            _unitOfWork.TestRepository.Update(_mapper.Map<Test>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task EnrollToTest(int userId, int testId)
        {
            var test = DeepCopy(await _unitOfWork.TestRepository.GetByIdWithDetailsAsync(testId).ConfigureAwait(false));
            test.Id = 0;
            test.UserId = userId;
            test.User = null;
            test.Owner = null;
            foreach (var themecount in test.ThemeCount)
            {
                themecount.Id = 0;
            }
            var testToAdd = _mapper.Map<TestModel>(test);
            await AddAsync(testToAdd);


        }

        private Test DeepCopy(Test test)
        {

            var str = JsonConvert.SerializeObject(test);

            return JsonConvert.DeserializeObject<Test>(str);

        }

        public async Task<List<TestModel>> CheckTestsAsync(int id, string name, DateTime fromDate, DateTime toDate)
        {
            var result = GetAllForOwner(id).Where(x => x.Name == name && x.StartDate > fromDate && x.EndDate < toDate && x.UserId != id).ToList();
            return result;
        }

        public async  Task<List<string>> GetTestsNames(int id)
        {
            var result = GetAllForOwner(id).Select(x=>x.Name).Distinct().ToList();
            return result;
        }

        public async Task<List<TestModel>> GetAllOwnerTests(int ownerId, string subject)
        {
            var result = GetAllForOwner(ownerId).Where(x => x.OwnerId == x.UserId&& x.Subject == subject);
            return result.ToList();
        }

        public async Task SendEmailsAsync(int id, string receivers, string link)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if(user.Surname==null||user.Name==null)
            {
                link = user.Email + " запрошує вас на тест\nПерейдіть за посиланням для зарахування: " + link;
            }
            else
            {
                link = user.Name+' ' + user.Surname + " запрошує вас на тест\nПерейдіть за посиланням для зарахування: " + link;
            }
            var receiver = receivers.Split('\n');
            foreach (var email in receiver)
            {
                SendEmail(link, "Запрошення на тест", email);
            }
        }

        private void SendEmail(string text, string theme, string receiverEmail)
        {
            SmtpClient MyServer = new SmtpClient();
            MyServer.Host = "smtp.gmail.com";
            MyServer.Port = 587;
            MyServer.EnableSsl = true;
            NetworkCredential NC = new NetworkCredential();
            NC.UserName = "medhelperteam@gmail.com";
            NC.Password = "345Edc345%%%";
            MyServer.Credentials = NC;

            MailAddress from = new MailAddress("MarkTestTeam@gmail.com", "MarkTestTeam");


            MailAddress receiver = new MailAddress(receiverEmail, receiverEmail);

            MailMessage Mymessage = new MailMessage(from, receiver);
            Mymessage.Subject = theme;
            Mymessage.Body = text;

            MyServer.Send(Mymessage);
        }

        public async Task<List<TestModel>> GetAllUserTests(int id)
        {
            var tests = GetAllForUser(id);
            return tests.ToList();
        }

        public async Task<List<string>> GetSubjects(int id)
        {
            var result = _unitOfWork.TestRepository.FindAll().Where(x => x.OwnerId == id).Select(x => x.Subject).Distinct().ToList();
            return result;
        }
    }
}
