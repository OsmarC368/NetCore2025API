using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> Login(User user);
        Task<User> Register(User user);
        Task<User> Update(int id, User user);
        Task<User> GetById(int id);
        Task<IEnumerable<User>> GetAll();
    }
}