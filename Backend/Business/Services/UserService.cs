using AutoMapper;
using Business.Extensions;
using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task AddAsync(UserModel model)
        {
            try
            {
                Validation.ValidateUser(model);
                await _unitOfWork.UserRepository.AddAsync(_mapper.Map<User>(model));
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponse> AuthenticateAsync(string username, string password)
        {
            var user = _unitOfWork.UserRepository.FindAll().FirstOrDefault(x => x.Login == username && x.Password == password);

            if (user == null)
                throw new MarkTestException("Invalid credentials");

            var userRole = (await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId)).RoleName;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userAuth = _mapper.Map<UserResponse>(user);
            userAuth.Token = tokenHandler.WriteToken(token);

            return userAuth;
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteByIdAsync(modelId);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _mapper.Map<List<UserModel>>(_unitOfWork.UserRepository.FindAll());
        }

        public async Task<UserModel> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task Register(UserModel user)
        {
            try
            {
                //Validation.ValidateUser(user);
                var userExist = _unitOfWork.UserRepository.FindAll().FirstOrDefault(x => x.Login == user.Login);

                if (userExist == null)
                {
                    user.Password = user.Password.HashPassword();
                    user.RoleId = _unitOfWork.RoleRepository.FindAll().FirstOrDefault(x => x.RoleName == "Student").Id;
                    await _unitOfWork.UserRepository.AddAsync(_mapper.Map<User>(user));
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    throw new MarkTestException("User with such login already exist.");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task UpdateAsync(UserModel model)
        {
            try
            {
                Validation.ValidateUser(model);
                model.Password = model.Password.HashPassword();
                //var checkUser = await _unitOfWork.UserRepository.GetByIdAsync(model.Id);
                //if(model.Password!=checkUser.Password)
                //{
                //    throw new MarkTestException("Invalid password");
                //}
                //checkUser = null;
                _unitOfWork.UserRepository.Update(_mapper.Map<User>(model));
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task MakeTeacher(int id)
        {
            try
            {

                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if(user==null)
                {
                    throw new ArgumentNullException();
                }
                user.RoleId = _unitOfWork.RoleRepository.FindAll().FirstOrDefault(x => x.RoleName == "Teacher").Id;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
