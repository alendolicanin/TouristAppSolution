using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TouristManagementService.DTOs;
using TouristManagementService.Models;

namespace TouristManagementService.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;

        public UserService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<User>> GetAllManagers()
        {
            var managers = await userManager.Users.Where(u => u.Role == Role.Manager).ToListAsync();
            return managers;
        }

        public async Task<List<User>> GetAllTourists()
        {
            var tourists = await userManager.Users.Where(u => u.Role == Role.Tourist).ToListAsync();
            return tourists;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return user;
        }

        public async Task<User> Login(UserLoginDTO userLoginDTO)
        {
            var user = await userManager.FindByNameAsync(userLoginDTO.UserName);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (await userManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                return user;
            }
            else
            {
                throw new Exception("Username and password not match");
            }
        }

        public async Task<User> Register(UserRegisterDTO userRegisterDTO)
        {
            var user = new User
            {
                UserName = userRegisterDTO.UserName,
                FirstName = userRegisterDTO.FirstName,
                LastName = userRegisterDTO.LastName,
                Age = userRegisterDTO.Age,
                PhoneNumber = userRegisterDTO.PhoneNumber,
                Role = userRegisterDTO.Role
            };

            var result = await userManager.CreateAsync(user, userRegisterDTO.Password);

            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                throw new Exception("Registration failed. Reason: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
