using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTAuthenticationAndAuthorizationWebAPI.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthenticationAndAuthorizationWebAPI.Controllers
{
    /// <summary>
    /// Authenticate Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Authenticate Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="configuration"></param>
        public AuthenticateController(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Login Authentication api
        /// </summary>
        /// <param name="model">login model Sagar/Test@123</param>
        /// <returns>JWT Token</returns>
        /// <response code="201">Returns a valid token</response>
        /// <response code="400">Requested object is null or empty</response> 
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) return BadRequest();

            var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isValidPassword) return Unauthorized();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model">Registration model</param>
        /// <returns>New created user</returns>
        /// <response code="201">Returns a newly created user</response>
        /// <response code="400">Requested object is null or empty</response> 
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                                new Response
                                {
                                    Status = "Error",
                                    Message = "User already exists!"
                                });

            var user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new Response
                                    {
                                        Status = "Error",
                                        Message = $"User creation failed! Please check user details and try again.{ result.Errors.ToString() }"
                                    });


            // Add Roles in Database
            await AddRolesInDatabaseIfNotExists();

            // Assign role to user
            await AssignRoleToUser(model, user);

            return Ok(new Response
            {
                Status = "Success",
                Message = "User created successfully!"
            });
        }

        private async Task AssignRoleToUser(RegisterModel model, ApplicationUser user)
        {
            switch (model.UserRole.ToLower())
            {
                case UserRoles.Admin:
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                    await _userManager.AddToRoleAsync(user, UserRoles.Manager);
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    break;
                case UserRoles.Manager:
                    await _userManager.AddToRoleAsync(user, UserRoles.Manager);
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    break;
                case UserRoles.User:
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                    break;
            }
        }

        /// <summary>
        /// Add roles in database if not exists
        /// </summary>
        /// <returns></returns>
        private async Task AddRolesInDatabaseIfNotExists()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        }
    }
}