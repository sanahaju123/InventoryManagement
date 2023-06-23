
using InventoryManagement.BusinessLayer.Interfaces;
using InventoryManagement.BusinessLayer.Services.Repository;
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InventoryManagement.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUser(User user)
        {
            return await _userRepository.CreateUser(user);
        }

        public async Task<User> DeleteUserById(int userId)
        {
            return await _userRepository.DeleteUserById(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<bool> LoginUser(User user)
        {
            return await _userRepository.LoginUser(user);
        }

        public async Task<User> SearchUserByName(string name)
        {
            return await _userRepository.SearchUserByName(name);
        }

        public async Task<User> UpdateUser(UserViewModel model)
        {
            return await _userRepository.UpdateUser(model);
        }
    }
}

