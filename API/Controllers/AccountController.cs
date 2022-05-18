using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using anxietyDiary.Controllers;
using Api.DTO;
using API.DTO;
using Api.Services;
using Domain.Enums;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using API.Core;

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


            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                user.Role = await _roleManager.FindByIdAsync(user.RoleId.ToString());
                return createUserObject(user);
            }
            return Unauthorized();
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _userManager.Users.AnyAsync(user => user.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Неверный email");
                return ValidationProblem();
            }
            if (await _userManager.Users.AnyAsync(user => user.UserName == registerDto.UserName))
            {
                ModelState.AddModelError("userName", "Неверное имя пользователя");
                return ValidationProblem();
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
                return createUserObject(user);
            }
            return BadRequest("problem register user");
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<UserDTO>> getCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null)
            {
                return new UserDTO
                {
                    Role = new RoleDTO
                    {
                        RoleName = RolesEnum.Guest.GetDescription()
                    },
                };
            }
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            user.Role = await _roleManager.FindByIdAsync(user.RoleId.ToString());

            return createUserObject(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> changeUser(UserDTO user)
        {
            var currentUser = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            changeUserEntity(ref currentUser, user);
            var result = await _userManager.UpdateAsync(currentUser);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("problem change user");
        }


        private UserDTO createUserObject(User user)
        {
            return new UserDTO
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Description = user.Description,
                isSearching = user.isSearching,
                Role = new RoleDTO
                {
                    RoleName = user.Role.Name
                },
                Token = _tokenService.CreateToken(user, _config)
            };
        }
        private void changeUserEntity(ref User user, UserDTO userDto)
        {
            user.isSearching = userDto.isSearching;
            user.FirstName = user.SecondName;
            user.SecondName = userDto.SecondName;
            user.Description = userDto.Description;
        }
    }
}