using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthRepoistory _AuthRepo ;
        public AuthController(IAuthRepoistory _authRepo)
        {
            _AuthRepo = _authRepo;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register (RegisterUserDto registerUserDto)
        {
            registerUserDto.userName =registerUserDto.userName.ToLower();
            if(await _AuthRepo.UserExists(registerUserDto.userName))
                return BadRequest("User Already Exists");

                var user = new User{
                    UserName = registerUserDto.userName
                };

            await _AuthRepo.Register(user,registerUserDto.Password);

            return StatusCode(201);            
        }
    }
}