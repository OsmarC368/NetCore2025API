﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        ValueTask<User> GetUser(string username, string password);
    }
}
