
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(User user);
        Task<bool> LoginUser(User user);
        Task<User> GetUserById(int userId);
        Task<User> SearchUserByName(string name);
        Task<User> DeleteUserById(int userId);
        Task<User> UpdateUser(UserViewModel model);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
