using MediumProject.EF_MediumProject_CodeFirst.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediumProject.Models.Model_Repository.Abstract
{
    public interface IUserVMRepository
    {
        User ConvertUserModelToUser(UserVM user);
        UserVM ConvertUserToUserModel(User user);
    }
}
