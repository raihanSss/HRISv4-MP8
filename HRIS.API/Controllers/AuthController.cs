using HRIS.Application.Services;
using HRIS.Domain.Interfaces;
using HRIS.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
   
    [Route("api/auth")]
    [ApiController]
    

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployeeModel model)
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(model);

                if (result.Status == "Success")
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (result.Status == "Error")
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("role")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] string rolename)
        {
            var result = await _authService.CreateRoleAsync(rolename);
            if (result.Status == "Error")
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel model)
        {
            var response = await _authService.RefreshTokenAsync(model);
            if (response.Status == "Success")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.UpdateUserAsync(userId, model);

            if (result.Status == "Success")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _authService.DeleteUserAsync(userId);

            if (result.Status == "Success")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpPost("register-all")]
        
        public async Task<IActionResult> RegisterAllEmployees()
        {
            try
            {
                await _authService.RegisterEmployeesAutomatically();
                return Ok(new { Status = "Success", Message = "All employees registered successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", Message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
