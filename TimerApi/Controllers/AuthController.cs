using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

using TimerApi.Models;

using TimerContext.Models;
using TimerContext.Models.Context;

namespace TimerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TimerDbContext _context;
        public AuthController(IConfiguration configuration, TimerDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (_context.Users.Any(x => x.Username.Equals(request.Username)))
            {
                return BadRequest($"User with nickname '{request.Username}' already exists.");
            }
            else
            {
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                try
                {
                    Statistics statistics = new()
                    {
                        LastUpdateTime = DateTime.UtcNow
                    };

                    _context.Statistics.Add(statistics);

                    await _context.SaveChangesAsync();

                    User user = new()
                    {
                        Username = request.Username,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        StatisticsId = statistics.StatisticsId,
                        UserPic = request.UserPic is null ? Array.Empty<byte>() : request.UserPic,
                        UserStatus = string.Empty,
                        DateRegister = DateTime.UtcNow,
                        LastSeen = DateTime.UtcNow,
                        UserIdentifier = Guid.NewGuid()
                    };

                    _context.Users.Add(user);

                    await _context.SaveChangesAsync();

                    return Ok(user);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserDto request)
        {
            User user = _context.Users.FirstOrDefault(x => x.Username.Equals(request.Username));

            if (user is null)
            {
                return BadRequest("User not found");
            }
            else
            {
                if (VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    try
                    {
                        string token = CreateToken(user);

                        request.Token = token;

                        return Ok(request);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else
                {
                    return BadRequest("Wrong password");
                }
            } 
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.SerialNumber, user.UserIdentifier.ToString())
            };

            SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:key").Value));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
