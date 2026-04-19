using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;
using AuthService.Application.DTOs;
using AuthService.Application;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Infrastructure.Services;
public class AuthService : IAuthService
{
    private readonly AuthDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AuthDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return "User registered";
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var keyString = _config["Jwt:Key"] ?? throw new Exception("JWT Key missing");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}