using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(QuestionModel model)
        {
            try
            {
                Validation.ValidateQuestion(model);
                var answer = new Answer
                {
                    Answers = new List<string>(model.Answers)
                };
                model.Answers.Clear();
                var element = _mapper.Map<Question>(model);
                await _unitOfWork.QuestionRepository.AddAsync(element);
                await _unitOfWork.SaveAsync();
                answer.QuestionId = element.Id;
                await _unitOfWork.AnswerRepository.AddAsync(answer);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CopyAsync(QuestionModel model)
        {
            try
            {
                Validation.ValidateQuestion(model);
                model.Privacy = true;
                var answer = _unitOfWork.AnswerRepository.FindAll().FirstOrDefault(x => x.QuestionId == model.Id);
                model.Id = 0;
                var element = _mapper.Map<Question>(model);
                await _unitOfWork.QuestionRepository.AddAsync(element);
                await _unitOfWork.SaveAsync();
                answer.QuestionId = element.Id;
                answer.Id = 0;
                await _unitOfWork.AnswerRepository.AddAsync(answer);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            try
            {
                await _unitOfWork.QuestionRepository.DeleteByIdAsync(modelId);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IEnumerable<QuestionModel> GetAllWithParams(string s,string sort_by, string sort_type, int offset, int limit)
        {
            if(s==null)
            {
                s = "";
            }
            var result = _unitOfWork.QuestionRepository.FindAll().Where(x => x.Privacy == false).ToList();
            if (result[0].GetType().GetProperty(sort_by).PropertyType == typeof(string))
            {
                result = result.OrderBy(x => GetPropertyValue(x, sort_by).ToString().ToLower()).ToList();
            }
            else
            {
                result = result.OrderBy(x => GetPropertyValue(x, sort_by)).ToList();
            }
            result = result.Where(x => QuestionToLowerString(x).Contains(s.ToLower())).ToList();
            if (sort_type == "desc")
            {
                result.Reverse();
            }
            if (limit == -1)
            {
                limit = result.Count();
            }
            result = result.Skip(offset * limit).Take(limit).ToList();
            return _mapper.Map<List<QuestionModel>>(result);

        }

        public async Task<QuestionModel> GetByIdAsync(int id)
        {
            var lot = await _unitOfWork.QuestionRepository.GetByIdWithDetailsAsync(id);
            if (lot == null)
            {
                throw new ArgumentNullException();
            }

            return _mapper.Map<QuestionModel>(lot);
        }

        public async Task UpdateAsync(QuestionModel model)
        {
            try
            {
                Validation.ValidateQuestion(model);
                _unitOfWork.QuestionRepository.Update(_mapper.Map<Question>(model));
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string QuestionToLowerString(Question obj)
        {

            return (obj.Description + obj.Name).ToLower();
        }
        private static object GetPropertyValue(Question obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj);
        }

        public IEnumerable<QuestionModel> GetAll()
        {
            return _mapper.Map<List<QuestionModel>>(_unitOfWork.QuestionRepository.FindAll().Where(x=>x.Privacy==false));
        }

        public IEnumerable<QuestionModel> GetAllByOwnerId(int id)
        {
            var result = _unitOfWork.QuestionRepository.FindAll().Where(x => x.OwnerId == id).ToList();
            return _mapper.Map<List<QuestionModel>>(result);
        }

        public IEnumerable<QuestionModel> GetAllByOwnerIdWithParams(int id, string s, string sort_by, string sort_type, int offset, int limit)
        {
            if (s == null)
            {
                s = "";
            }
            var result = _unitOfWork.QuestionRepository.FindAll().Where(x => x.OwnerId == id).ToList();
            if (result[0].GetType().GetProperty(sort_by).PropertyType == typeof(string))
            {
                result = result.OrderBy(x => GetPropertyValue(x, sort_by).ToString().ToLower()).ToList();
            }
            else
            {
                result = result.OrderBy(x => GetPropertyValue(x, sort_by)).ToList();
            }
            result = result.Where(x => QuestionToLowerString(x).Contains(s.ToLower())).ToList();
            if (sort_type == "desc")
            {
                result.Reverse();
            }
            if (limit == -1)
            {
                limit = result.Count();
            }
            result = result.Skip(offset * limit).Take(limit).ToList();
            return _mapper.Map<List<QuestionModel>>(result);
        }

        public IEnumerable<string> GetSubjects(int id)
        {
            return _unitOfWork.QuestionRepository.FindAll().Where(x => x.OwnerId == id).Select(x => x.Subject).Distinct().ToList();
        }

        public IEnumerable<string> GetThemesForSubject(int id, string subject)
        {
            return _unitOfWork.QuestionRepository.FindAll().Where(x => x.OwnerId == id&&x.Subject==subject).Select(x => x.Theme).Distinct().ToList();
        }
    }
}
