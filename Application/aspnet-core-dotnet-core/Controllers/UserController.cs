using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using aspnet_core_dotnet_core.Helper;

namespace aspnet_core_dotnet_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration configuration;
        private JsonResponser responser = new JsonResponser();

        public UserController(DataContext context, IConfiguration configuration)
        {
            this._context = context;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var hasUser = _context.Users.SingleOrDefault(user => user.Username == request.Username);
            if (hasUser != null)
                return BadRequest(responser.response("User already exists."));
            createPasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User();
            user.Username = request.Username;
            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(responser.response("Registration successful", user));
        }

        [HttpPost("login")]
        public ActionResult<JsonResponser> login(UserDTO request)
        {
            var user = _context.Users.SingleOrDefault(user => user.Username == request.Username);
            if (user == null)                
                return BadRequest(responser.response("User not found."));

            if (!verifyPasswordHash(request.Password, user.Password, user.PasswordSalt))
                return BadRequest(responser.response("Password incorrect."));
            string token = createToken(user);

            return Ok(responser.response("Login successful", new Dictionary<string, string> { { "access", token } }));
        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string createToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
