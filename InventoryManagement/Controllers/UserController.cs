using InventoryManagement.BusinessLayer.Interfaces;
using InventoryManagement.BusinessLayer.Services;
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SchedulerService _scheduler;
        public UserController(IUserService userService, SchedulerService scheduler)
        {
            _userService = userService;
            _scheduler = scheduler;
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            var userExists = await _userService.GetUserById(model.Id);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            User user = new User()
            {
                UserName = model.UserName,Id= model.Id,FirstName=model.FirstName,lastName=model.lastName,IsDeleted=model.IsDeleted,Password=model.Password,Phone=model.Phone
            };
            var result = await _userService.CreateUser(user);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });

        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel model)
        {
            var user = await _userService.GetUserById(model.Id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"User With Id = {model.Id} cannot be found" });
            }
            else
            {
                var result = await _userService.UpdateUser(model);
                return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
            }
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _userService.DeleteUserById(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"User With Id = {id} cannot be found" });
            }
            else
            {
                var result = await _userService.DeleteUserById(id);
                return Ok(new Response { Status = "Success", Message = "User deleted successfully!" });
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"User With Id = {userId} cannot be found" });
            }
            else
            {
                return Ok(user);
            }
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] User user)
        {
            bool data = await _userService.LoginUser(user);
            if (data == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"User With Id = {user.Id} cannot be found" });
            }
            else
            {
                return Ok(data);
            }
        }

        /// <summary>
        /// Search User By name
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchUserByName")]
        public async Task<IActionResult> SearchUserByName(int userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"User With Id = {userId} cannot be found" });
            }
            else
            {
                return Ok(user);
            }
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        /// <summary>
        /// Start scheduler
        /// </summary>
        /// <returns></returns>
        [HttpGet("start-scheduler")]
        public IActionResult StartScheduler()
        {
            _scheduler.Start();
            return Ok();
        }

        /// <summary>
        /// Stop Scheduler
        /// </summary>
        /// <returns></returns>
        [HttpGet("stop-scheduler")]
        public IActionResult StopScheduler()
        {
            _scheduler.Stop();
            return Ok();
        }
    }
}
