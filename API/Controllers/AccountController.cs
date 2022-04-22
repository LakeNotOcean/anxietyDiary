using System.Security.Claims;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using Api.DTO;
using API.DTO;
using API.Services;
using Domain.Enums;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 RoleManager<Role> roleManager,
                                 TokenService tokenService,
                                 IConfiguration config,
                                 ILogger<AccountController> logger)
        {
            _roleManager = roleManager;
            _config = config;
            _logger = logger;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.email);
            user.Role = await _roleManager.FindByIdAsync(user.RoleId.ToString());

            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return new UserDTO
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Role = new RoleDTO
                    {
                        RoleName = user.Role.Name
                    },
                    Token = _tokenService.CreateToken(user, _config)
                };
            }
            return Unauthorized();
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _userManager.Users.AnyAsync(user => user.Email == registerDto.Email))
            {
                return BadRequest("Email уже используется");
            }
            if (await _userManager.Users.AnyAsync(user => user.UserName == registerDto.UserName))
            {
                return BadRequest("Имя пользователя уже используется");
            }
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                SecondName = registerDto.SecondName,
                Role = await _roleManager.Roles.FirstAsync(role => role.Name == RolesEnum.Patient.GetDescription())

            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                return new UserDTO
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Role = new RoleDTO
                    {
                        RoleName = user.Role.Name
                    },
                    Token = _tokenService.CreateToken(user, _config)
                };
            }
            return BadRequest("problem register user");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> getCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return await createUserObject(user);

        }
        private async Task<ActionResult<UserDTO>> createUserObject(User user)
        {
            return new UserDTO
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Role = new RoleDTO
                {
                    RoleName = (await _roleManager.FindByIdAsync(user.RoleId.ToString())).Name
                },
                Token = _tokenService.CreateToken(user, _config)
            };
        }
    }
}