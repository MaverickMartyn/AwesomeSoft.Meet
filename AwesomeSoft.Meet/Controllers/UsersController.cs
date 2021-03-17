using AwesomeSoft.Meet.Helpers;
using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #region Actions
        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="model">The model containing info required to authenticate.</param>
        /// <returns>A <see cref="AuthenticateResponse"/> containing user information and JWToken.</returns>
        /// <response code="200">On a successful authentication.</response>
        /// <response code="400">Failed to authenticate.</response>
        [HttpPost("api/[controller]/[action]")]
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// <returns>A list of all <see cref="User"/> instances.</returns>
        /// <response code="200">Success.</response>
        [Authorize]
        [HttpGet("api/[controller]")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(_userService.GetAll());
        }

        /// <summary>
        /// Gets a specific user by its ID.
        /// </summary>
        /// <returns>The requested <see cref="User"/> or null.</returns>
        /// <response code="200">Success.</response>
        /// <response code="404">Not found.</response>
        [Authorize]
        [HttpGet("api/[controller]/{id?}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public IActionResult Get(uint id)
        {
            var user = _userService.GetById(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        #endregion
    }
}
