using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MS.Services.AuthAPI.Data;
using MS.Services.AuthAPI.Models;
using MS.Services.AuthAPI.Models.Dto;
using MS.Services.AuthAPI.Service.IService;

namespace MS.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;

        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if(user != null) 
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    // Create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if(user == null || isValid == false) 
            {
                return new LoginResponseDto() { User = null, Token = ""};
            }
            // TODO : Generate Jwt token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);
            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = registrationRequestDto.Email,
                    Email = registrationRequestDto.Email,
                    NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                    Name = registrationRequestDto.Name,
                    PhoneNumber = registrationRequestDto.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if(result.Succeeded)
                {
                    var userToReturn = await _db.ApplicationUsers.FirstAsync(x => x.Email == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        Id = userToReturn.Id,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
