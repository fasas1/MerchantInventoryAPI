//using AutoMapper;
//using MechantInventory.Data;
//using MechantInventory.Model;
//using MechantInventory.Models.Dto;
//using MechantInventory.Repository.IRepository;
//using MechantInventory.Utility;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;


//namespace MechantInventory.Repository
//{
//    public class AuthRepository : IAuthRepository
//    {
//        private readonly ApplicationDbContext _db;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly string _secretKey;
//        private readonly IMapper _mapper;

//        public AuthRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper,
//                        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            _db = db;
//            _userManager = userManager;
//            _mapper = mapper;
//            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
//            _roleManager = roleManager;
//        }

//        public async Task<UserDto> Register(RegisterRequestDto registerRequestDto)
//        {
//            var user = new ApplicationUser
//            {
//                UserName = registerRequestDto.UserName,
//                Email = registerRequestDto.UserName,
//                NormalizedEmail = registerRequestDto.UserName.ToUpper(),
//                FullName = registerRequestDto.Name
//            };

//            try
//            {
//                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
//                if (result.Succeeded)
//                {
//                    // Assign default role
//                    string selectedRole = string.IsNullOrEmpty(registerRequestDto.Role) ? SD.Role_Staff : registerRequestDto.Role;

//                    // Ensure role exists before assigning
//                    if (!await _roleManager.RoleExistsAsync(selectedRole))
//                    {
//                        await _roleManager.CreateAsync(new IdentityRole(selectedRole));
//                    }

//                    await _userManager.AddToRoleAsync(user, selectedRole);

//                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registerRequestDto.UserName);

//                    UserDto userDto = new()
//                    {
//                        UserName = userToReturn.Email,
//                        ID = userToReturn.Id,
//                        FullName = userToReturn.FullName
//                    };

//                    return userDto;
//                }
//            }
//            catch (Exception e)
//            {
//                throw new Exception("An error occurred while registering the user", e);
//            }

//            return new UserDto();
//        }

//        //public async Task<bool> AssignRole(string email, string roleName)     
//        //{
//        //    var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
//        //    if(user != null)
//        //    {
//        //      if(!await _roleManager.RoleExistsAsync(SD.Role_Admin))
//        //        {
//        //            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
//        //        }
//        //      if (!await _roleManager.RoleExistsAsync(SD.Role_Staff))
//        //        {
//        //            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Staff));
//        //        }

//        //        // Assign role to user
//        //      if (roleName.ToLower() == SD.Role_Admin.ToLower())
//        //        {
//        //            await _userManager.AddToRoleAsync(user, SD.Role_Admin);
//        //        }
//        //        else if (roleName.ToLower() == SD.Role_Staff.ToLower())
//        //        {
//        //            await _userManager.AddToRoleAsync(user, SD.Role_Staff);
//        //        }
//        //        else
//        //        {
//        //            // Handle the case when the role name doesn't match any known roles
//        //            return false;
//        //        }

//        //        return true; 
//        //    }

//        //    return false;
//        //}
        

//        public bool IsUniqueUser(string username)
//        {
//            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
//              return user == null;
//        }

//        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
//        {
//            var user = _db.ApplicationUsers
//                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

//            if (user == null)
//            {
//                return new LoginResponseDto { Token = "", User = null };
//            }

//            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

//            if (!isValid)
//            {
//                return new LoginResponseDto { Token = "", User = null };
//            }

//            var roles = await _userManager.GetRolesAsync(user);
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_secretKey);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//            new Claim(ClaimTypes.Name, user.UserName),
//            new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
//        }),
//                Expires = DateTime.UtcNow.AddDays(7),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var loginResponseDTO = new LoginResponseDto
//            {
//                Token = tokenHandler.WriteToken(token),
//                User = _mapper.Map<UserDto>(user)  // AutoMapper will now be able to map
//            };
//            return loginResponseDTO;
//        }

        
//    }
//}
