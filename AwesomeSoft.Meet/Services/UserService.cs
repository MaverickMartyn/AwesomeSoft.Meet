using AwesomeSoft.Meet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using AwesomeSoft.Meet.Helpers;

namespace AwesomeSoft.Meet.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(uint id);

        User GetCurrentUser();
    }

    public class UserService : IUserService
    {
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Users hardcoded for simplicity, store in a db with hashed passwords in production applications.
        // Also readonly, mainly to shut up the compiler as the dummy data never changes.
        private readonly List<User> _users = DummyData.Instance.Users;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Methods
        /// <summary>
        /// Gets the currently logged in <see cref="User"/>.
        /// </summary>
        /// <returns>The <see cref="User"/> or null.</returns>
        public User GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext.Items["User"] as User;
        }

        /// <summary>
        /// Authenticate a <see cref="User"/> and generate its Jjon Web Token.
        /// </summary>
        /// <param name="model">Container for authentication information.</param>
        /// <returns>A <see cref="AuthenticateResponse"/> containing <see cref="User"/> details and JWT.</returns>
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Name == model.Username);
            //var user = _users.SingleOrDefault(x => x.Name == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        /// <summary>
        /// Get all <see cref="User"/>s.
        /// </summary>
        /// <returns>An IEnumerable og <see cref="User"/>s</returns>
        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        /// <summary>
        /// Get a <see cref="User"/> by its ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>A <see cref="User"/> or null.</returns>
        public User GetById(uint id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }
        #endregion

        #region Helper methods
        private static string GenerateJwtToken(User user)
        {
            // Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("my_secret_app_token");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
