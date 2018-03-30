﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstract
{
    public interface IDataRepository
    {
        IEnumerable<User> GetUsers();

        User FindUser(int id);

        void AddUser(User user);

        void DeleteUser(User user);

        IEnumerable<Exercise> GetExercises();
    }
}