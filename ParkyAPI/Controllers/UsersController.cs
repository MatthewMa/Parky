using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User user)
        {
            var obj = _userRepository.Authenticate(user.UserName, user.Password);
            if (obj == null)
            {
                return BadRequest(new { message = "Username or password is incorrect." });
            }        
            return Ok(obj);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            bool unique = _userRepository.IsUniqueUser(user.UserName);
            if (!unique)
            {
                return BadRequest(new { message = "Username already exists." });
            }
            var obj = _userRepository.Register(user.UserName, user.Password);
            if (obj == null)
            {
                return BadRequest(new { message = "Error while register." });
            }
            return Ok();
        }

    }
}
