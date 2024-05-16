using CarRentalManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _configuration;

    public AuthenticationController(ICustomerRepository customerRepository, IConfiguration configuration)
    {
        _customerRepository = customerRepository;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var (isValid, role, userId) = await _customerRepository.CheckCredentialsAsync(loginDto.Email, loginDto.Password);
        if (isValid)
        {
            var token = GenerateJwtToken(loginDto.Email, role, userId);
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }

    private string GenerateJwtToken(string email, string role, int userId)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("userId", userId.ToString()), // Add userId to claims
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
