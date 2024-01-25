using TouristManagementService.DTOs;
using TouristManagementService.Models;

namespace TouristManagementService.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllTourists();
        Task<List<User>> GetAllManagers();
        Task<User> GetUserById(string id);
        Task<User> Register(UserRegisterDTO userRegisterDTO);
        Task<User> Login(UserLoginDTO userLoginDTO);
    }
}
