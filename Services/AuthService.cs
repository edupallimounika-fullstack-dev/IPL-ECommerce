using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IPL.ECommerce.Configuration;
using IPL.ECommerce.Data;
using IPL.ECommerce.Domain.Entities;
using IPL.ECommerce.DTOs;
using IPL.ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IPL.ECommerce.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(
        ApplicationDbContext context,
        IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<LoginResponse> RegisterAsync(
        RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var existingUser =
            await _context.Users
                .AnyAsync(x => x.Email == email);

        if (existingUser)
        {
            throw new InvalidOperationException(
                "An account with this email already exists.");
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email
        };

        user.PasswordHash =
            _passwordHasher.HashPassword(
                user,
                request.Password);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return CreateLoginResponse(user);
    }

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request)
    {
        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        var user =
            await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.IsActive);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        var result =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        return CreateLoginResponse(user);
    }

    private LoginResponse CreateLoginResponse(
        User user)
    {
        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new(
                ClaimTypes.Email,
                user.Email),

            new(
                ClaimTypes.Name,
                $"{user.FirstName} {user.LastName}")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _jwtSettings.Key));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpirationMinutes),
            signingCredentials: credentials);

        return new LoginResponse
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            Email = user.Email,
            Token = new JwtSecurityTokenHandler()
                .WriteToken(token)
        };
    }
}