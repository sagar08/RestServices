using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Dtos;
using JWTPolicyBasedAuthorization.Infrastructure;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTPolicyBasedAuthorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region Member Variables
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IConfiguration _configuration;
        #endregion

        #region  Constructor
        /// <summary>
        /// Auth Controller Constructor
        /// </summary>
        /// <param name="userRepo">User Repository</param>
        /// <param name="roleRepo">Role Repository</param>
        /// <param name="configuration">Configuration Properties</param>
        public AuthController(IUserRepository userRepo, IRoleRepository roleRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _configuration = configuration;
        }
        #endregion

        #region Api Methods

        /// <summary>
        /// Login User based on valid credientials
        /// </summary>
        /// <param name="userDto">Login User Dto</param>
        /// <returns>JWT token</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Check user name exists
            var user = await _userRepo.FindByCondition(x => x.UserName == userDto.UserName).FirstOrDefaultAsync();
            if (user == null) return BadRequest();

            // Check for valid password
            var isValidPassword = new CommonUtility().ValidatePassword(userDto.Password, user.PasswordHash, user.PasswordSalt);
            if (!isValidPassword) return Unauthorized();

            // Create Claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var roles = await _roleRepo.GetUserRolesAsync(user.Id);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.RoleName));

                if (role.CanCreate)
                    authClaims.Add(new Claim("CanCreate", true.ToString()));

                if (role.CanDelete)
                    authClaims.Add(new Claim("CanDelete", true.ToString()));

                if (role.CanApprove)
                    authClaims.Add(new Claim("CanApprove", true.ToString()));

                if (role.CanView)
                    authClaims.Add(new Claim("CanView", true.ToString()));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512)
            );

            return Ok(
                new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
        }


        /// <summary>
        /// Register new User
        /// </summary>
        /// <param name="userDto">Register User Dto</param>
        /// <returns>Create a new User</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Check if User name already exists
            var user = await _userRepo.GetUserByNameAsync(userDto.UserName);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorDto
                {
                    Status = "Error",
                    Message = $"User with the name {userDto.UserName} already exists!"

                });
            }

            // Insert new user in database
            var newUser = GetUser(userDto);
            var role = await _roleRepo.GetRoleByNameAsync(userDto.RoleName);

            var result = await _userRepo.RegisterUserAsync(newUser, role);

            if (result != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new ErrorDto
                {
                    Status = "Error",
                    Message = $"User creation failed! Please check user details and try again."
                });
            }

            return Ok(
                new ResponseDto<RegisterUserDto>
                {
                    Data = userDto,
                    Status = "Success",
                    Message = "User created successfully!"
                }
            );
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Get User instance from dto object
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        private User GetUser(RegisterUserDto userDto)
        {
            User newUser = new User
            {
                UserName = userDto.UserName
            };

            (byte[] passwordHash, byte[] passwordSalt) = new CommonUtility().CreatePasswordHash(userDto.Password);

            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            return newUser;
        }
        #endregion
    }
}