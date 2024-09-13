using HRIS.Domain.Dtos;
using HRIS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Domain.Interfaces
{
    public interface IAuthService
    {
        
        Task<ResponeModel> RegisterAsync(RegisterEmployeeModel registerModel);
        Task<LoginResponseDto> LoginAsync(LoginModel loginModel);

        Task<AuthRespone> CreateRoleAsync(string rolename);

        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestModel model);

        Task<ResponeModel> UpdateUserAsync(string userId, UpdateUserModel model);
        Task<ResponeModel> DeleteUserAsync(string userId);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();

        Task RegisterEmployeesAutomatically();
    }

}
