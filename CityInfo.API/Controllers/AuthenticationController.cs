using CityInfo.API.Entities;
using CityInfo.API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers;

[Route("api/authentication")]
[ApiController]

public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AuthenticationController(IConfiguration configuration)
    {

        _configuration = configuration;

    }

    [HttpPost("authenticate")]
    public ActionResult<string> Authentication(AuthenticationRequestBody authenticationRequestBody)
    {
        var user = ValidationUserCredentials.ValidationUserCredential(authenticationRequestBody.UserName,
            authenticationRequestBody.Password);
        if (user == null)
        {
            return Unauthorized();
        }
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
        var signingCredentials = new SigningCredentials(
                        securityKey, SecurityAlgorithms.HmacSha256
                        );
        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("userId", user.UserId.ToString()));
        claimsForToken.Add(new Claim("NameKarbari", user.FirstName.ToString()));
        claimsForToken.Add(new Claim(ClaimTypes.Email, user.City.ToString()));

        var jwtSecurityToke = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.Now,
            DateTime.Now.AddHours(1),
            signingCredentials
            );

        var tokenToReturn = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToke);
        return Ok(tokenToReturn);
    }

}
