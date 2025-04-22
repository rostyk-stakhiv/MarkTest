using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Question, QuestionModel>()
                .ForMember(p => p.Id, c => c.MapFrom(question => question.Id))
                .ForMember(p => p.Name, c => c.MapFrom(question => question.Name))
                .ForMember(p => p.Description, c => c.MapFrom(question => question.Description))
                .ForMember(p => p.Owner, c => c.MapFrom(question => question.Owner))
                .ForMember(p => p.DificultyLevel, c => c.MapFrom(question => question.DificultyLevel))
                .ForMember(p => p.OwnerId, c => c.MapFrom(question => question.OwnerId))
                .ForMember(p => p.Type, c => c.MapFrom(question => question.Type))
                .ForMember(p => p.Privacy, c => c.MapFrom(question => question.Privacy))
                .ForMember(p => p.Owner, c => c.MapFrom(question => question.Owner))
                .ForMember(p => p.Theme, c => c.MapFrom(question => question.Theme))
                .ForMember(p => p.Subject, c => c.MapFrom(question => question.Subject))
                .ForMember(p => p.Answers, c => c.MapFrom(question => question.Answers))
                .ForMember(p => p.Mark, c => c.MapFrom(question => question.Mark))
                .ForMember(p => p.SuggestedAnswers, c => c.MapFrom(question => question.SuggestedAnswers))
                .ReverseMap();

            CreateMap<ThemeCount, ThemeCountModel>()
                .ForMember(p => p.Theme, c => c.MapFrom(themecount => themecount.Theme))
                .ForMember(p => p.Count, c => c.MapFrom(themecount => themecount.Count))
                .ForMember(p => p.TestId, c => c.MapFrom(themecount => themecount.TestId))
                .ForMember(p => p.Id, c => c.MapFrom(themecount => themecount.Id))
                .ReverseMap();

            CreateMap<User, UserResponse>()
                .ForMember(p => p.Id, c => c.MapFrom(user => user.Id))
                .ForMember(p => p.Name, c => c.MapFrom(user => user.Name))
                .ForMember(p => p.Surname, c => c.MapFrom(user => user.Surname))
                .ForMember(p => p.Phone, c => c.MapFrom(user => user.Phone))
                .ForMember(p => p.Email, c => c.MapFrom(user => user.Email))
                .ForMember(p => p.RoleId, c => c.MapFrom(user => user.RoleId))
                .ForMember(p => p.Role, c => c.MapFrom(user => new Role { Id = user.RoleId, RoleName = user.Role.RoleName }))
                .ForMember(p => p.Login, c => c.MapFrom(user => user.Login))
                .ForMember(p => p.Password, c => c.MapFrom(user => user.Password));


            CreateMap<UserAuthenticate, User>()
                .ForMember(p => p.Login, c => c.MapFrom(user => user.Login))
                .ForMember(p => p.Password, c => c.MapFrom(user => user.Password))
                .ReverseMap();

            CreateMap<User, UserModel>()
                .ForMember(p => p.Id, c => c.MapFrom(user => user.Id))
                .ForMember(p => p.Name, c => c.MapFrom(user => user.Name))
                .ForMember(p => p.Surname, c => c.MapFrom(user => user.Surname))
                .ForMember(p => p.Phone, c => c.MapFrom(user => user.Phone))
                .ForMember(p => p.Email, c => c.MapFrom(user => user.Email))
                .ForMember(p => p.Role, c => c.MapFrom(user => user.Role))
                .ForMember(p => p.RoleId, c => c.MapFrom(user => user.RoleId))
                .ForMember(p => p.Login, c => c.MapFrom(user => user.Login))
                .ForMember(p => p.Password, c => c.MapFrom(user => user.Password))
                .ReverseMap();

            CreateMap<Test, TestModel>()
                .ForMember(p => p.Id, c => c.MapFrom(test => test.Id))
                .ForMember(p => p.Questions, c => c.MapFrom(test => test.Questions))
                .ForMember(p => p.Subject, c => c.MapFrom(test => test.Subject))
                .ForMember(p => p.DificultyLevel, c => c.MapFrom(test => test.DificultyLevel))
                .ForMember(p => p.Durability, c => c.MapFrom(test => test.Durability))
                .ForMember(p => p.EndDate, c => c.MapFrom(test => test.EndDate))
                .ForMember(p => p.Mark, c => c.MapFrom(test => test.Mark))
                .ForMember(p => p.MaxMark, c => c.MapFrom(test => test.MaxMark))
                .ForMember(p => p.NumberOfAttempts, c => c.MapFrom(test => test.NumberOfAttempts))
                .ForMember(p => p.StartDate, c => c.MapFrom(test => test.StartDate))
                .ForMember(p => p.ThemeCount, c => c.MapFrom(test => test.ThemeCount))
                .ForMember(p => p.UserId, c => c.MapFrom(test => test.UserId))
                .ForMember(p => p.OwnerId, c => c.MapFrom(test => test.OwnerId))
                .ForMember(p => p.Owner, c => c.MapFrom(test => test.Owner))
                .ForMember(p => p.User, c => c.MapFrom(test => test.User))
                .ForMember(p => p.Name, c => c.MapFrom(test => test.Name))
                .ReverseMap();

        }
    }
}