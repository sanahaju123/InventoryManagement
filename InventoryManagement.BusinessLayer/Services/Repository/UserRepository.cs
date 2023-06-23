using InventoryManagement.BusinessLayer.Services.Repository;
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.DataLayer;
using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InventoryManagement.BusinessLayer.Services.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly InventoryDbContext _inventoryDbContext;
        public UserRepository(InventoryDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                var result = await _inventoryDbContext.Users.AddAsync(user);
                await _inventoryDbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<User> DeleteUserById(int userId)
        {
            var user = await _inventoryDbContext.Users.FindAsync(userId);
            try
            {
                user.IsDeleted = false;

                _inventoryDbContext.Users.Update(user);
                await _inventoryDbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var result = _inventoryDbContext.Users.
                OrderByDescending(x => x.Id).Where(x => x.IsDeleted == false).Take(10).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
       

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                return await _inventoryDbContext.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<bool> LoginUser(User user)
        {
            try
            {
                var data=  _inventoryDbContext.Users.SingleOrDefault(c=>c.UserName==user.UserName && c.IsDeleted==true);
                if(data!=null && (data.UserName==user.UserName && data.Password == user.Password))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw (ex);
                return false;
            }

        }

        public async Task<User> SearchUserByName(string name)
        {
            try
            {
                return await _inventoryDbContext.Users.FindAsync(name);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<User> UpdateUser(UserViewModel model)
        {
            var user = await _inventoryDbContext.Users.FindAsync(model.Id);
            try
            {
                user.Id = model.Id;
                user.FirstName = model.FirstName;
                user.lastName = model.lastName;
                user.UserName = model.UserName;
                user.Password = model.Password;
                user.Phone = model.Phone;
                user.IsDeleted = model.IsDeleted;

                _inventoryDbContext.Users.Update(user);
                await _inventoryDbContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}