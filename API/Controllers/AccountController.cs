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
using Microsoft.AspNetCore.Http;
using System;

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
                await setRefreshToken(user);
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
                await setRefreshToken(user);
                return createUserObject(user);
            }
            return BadRequest("problem register user");
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDTO>> getCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null)
            {
                return new UserDTO
                {
                    role = RolesEnum.Guest.GetDescription()

                };
            }
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            user.Role = await _roleManager.FindByIdAsync(user.RoleId.ToString());

            return createUserObject(user);
        }

        [Authorize]
        [HttpPost("changeinfo")]
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

        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users.Include(r => r.RefreshTokens)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null)
            {
                return Unauthorized();
            }
            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
            if (oldToken is not null && !oldToken.IsActive)
            {
                return Unauthorized();
            }
            return createUserObject(user);

        }


        private UserDTO createUserObject(User user)
        {
            return new UserDTO
            {
                userName = user.UserName,
                firstName = user.FirstName,
                secondName = user.SecondName,
                description = user.Description,
                isSearching = user.isSearching,
                role = user.Role.Name,
                token = _tokenService.CreateToken(user, _config)
            };
        }
        private void changeUserEntity(ref User user, UserDTO userDto)
        {
            user.isSearching = userDto.isSearching;
            user.FirstName = userDto.firstName;
            user.SecondName = userDto.secondName;
            user.Description = userDto.description;
        }

        private async Task setRefreshToken(User user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}