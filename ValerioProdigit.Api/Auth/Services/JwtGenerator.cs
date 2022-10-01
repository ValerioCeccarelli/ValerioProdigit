using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ValerioProdigit.Api.Configurations;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Auth.Services;

public sealed class JwtGenerator
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public JwtGenerator(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }

    public async Task<string> GenerateAsync(ApplicationUser user)
    {
        var claims = new List<Claim>(6)
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
		
        //this app don't use user claims
        //claims.AddRange(await _userManager.GetClaimsAsync(user));
		
        var userRole = await _userManager.GetRolesAsync(user);
        claims.AddRange(userRole.Select(role => new Claim(ClaimTypes.Role, role)));

        var secureKey = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;
        var securityKey = new SymmetricSecurityKey(secureKey);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.Add(_jwtSettings.ExpiryTime),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = credentials,
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }
}