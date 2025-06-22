using HongDucFashion.ModelDTO;
using HongDucFashion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
namespace HongDucFashion.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HongDucFashionV1Context _db;

        public AuthController(HongDucFashionV1Context db)
        {
            _db = db;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required.");

            var user = await _db.UserAccounts
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return Unauthorized("Invalid credentials.");

            var hash = ComputeSha256Hash(request.Password);
            if (user.PasswordHash != hash)
                return Unauthorized("Invalid credentials.");
 
            return Ok(new AuthResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles?.Select(r => r.RoleName).ToList() ?? new List<string>()
            });
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("All fields are required.");

            var exists = await _db.UserAccounts.AnyAsync(u => u.Email == request.Email);
            if (exists)
                return Conflict("Email already exists.");

            var user = new UserAccount
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = ComputeSha256Hash(request.Password)
            };

            _db.UserAccounts.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        // GET: api/Auth/me/{id}
        [HttpGet("me/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _db.UserAccounts.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new AuthResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        // Utility: SHA256 hash
        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}