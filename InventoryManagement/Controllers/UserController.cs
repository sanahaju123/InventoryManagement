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

        [HttpGet("start-scheduler")]
        public IActionResult StartScheduler()
        {
            _scheduler.Start();
            return Ok();
        }

        [HttpGet("stop-scheduler")]
        public IActionResult StopScheduler()
        {
            _scheduler.Stop();
            return Ok();
        }

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

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }
    }
}
