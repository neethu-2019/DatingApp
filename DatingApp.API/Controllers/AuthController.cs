using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthRepoistory _AuthRepo;

        public IConfiguration _Config ;

        public AuthController(IAuthRepoistory _authRepo, IConfiguration config)
        {
            _AuthRepo = _authRepo;
            _Config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            registerUserDto.userName = registerUserDto.userName.ToLower();
            if (await _AuthRepo.UserExists(registerUserDto.userName))
                return BadRequest("User Already Exists");

            var user = new User
            {
                UserName = registerUserDto.userName
            };

            await _AuthRepo.Register(user, registerUserDto.Password);

            return StatusCode(201);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var userToLogin = await _AuthRepo.Login(loginUserDto.UserName.ToLower(), loginUserDto.Password);
            if (userToLogin == null)
                return Unauthorized();
            var claims = new[]
            {
              new Claim(ClaimTypes.NameIdentifier, userToLogin.Id.ToString()),
              new Claim(ClaimTypes.Name,userToLogin.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

        var tokenDesciptors = new SecurityTokenDescriptor
        {
            Subject =new ClaimsIdentity(claims),
            Expires  = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var jwtToken = new JwtSecurityTokenHandler();

        var token = jwtToken.CreateJwtSecurityToken(tokenDesciptors);

        return Ok(new {
            token =jwtToken.WriteToken(token)
        });

        }
    }
}