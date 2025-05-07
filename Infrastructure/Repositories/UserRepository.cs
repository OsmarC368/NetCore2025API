using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) {}

        public async ValueTask<User> GetUser(string username, string password)
        {
            return await base.dbSet.FirstAsync(user => user.UserName == username  && user.Password == password);
        }
        
    }
}