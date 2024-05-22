using CarRentalManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _configuration;

    // Constructor to inject dependencies
    public AuthenticationController(ICustomerRepository customerRepository, IConfiguration configuration)
    {
        _customerRepository = customerRepository;
        _configuration = configuration;
    }

    // Login endpoint to authenticate users
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Check user credentials
        var (isValid, role, userId) = await _customerRepository.CheckCredentialsAsync(loginDto.Email, loginDto.Password);
        if (isValid)
        {
            // Generate JWT token if credentials are valid
            var token = GenerateJwtToken(loginDto.Email, role, userId);
            return Ok(new { Token = token });
        }
        return Unauthorized(); // Return 401 if credentials are invalid
    }

    // Method to generate a JWT token
    private string GenerateJwtToken(string email, string role, int userId)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Define claims to include in the token
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // Use userId as sub
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim("userId", userId.ToString()), // Add userId to claims
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Create the JWT token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token); // Return the serialized token
    }

    // Endpoint to refresh JWT tokens
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        // Get principal from expired token
        var principal = GetPrincipalFromExpiredToken(tokenRequest.Token);
        if (principal == null)
        {
            return BadRequest("Invalid token"); // Return 400 if token is invalid
        }

        // Extract claims from principal
        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        var role = principal.FindFirst(ClaimTypes.Role)?.Value;
        var userId = int.Parse(principal.FindFirst("userId").Value);

        // Optionally, validate the refresh token here

        // Generate new JWT and refresh tokens
        var newToken = GenerateJwtToken(email, role, userId);
        var newRefreshToken = Guid.NewGuid().ToString(); // Generate a new refresh token

        // Save the new refresh token in the database or other storage

        return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
    }

    // Method to validate and get principal from an expired token
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ValidateLifetime = false // Ignore the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is JwtSecurityToken jwtSecurityToken &&
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return principal; // Return the claims principal if token is valid
        }

        throw new SecurityTokenException("Invalid token"); // Throw exception if token is invalid
    }

    // DTO for token request
    public class TokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

// DTO for login request
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
