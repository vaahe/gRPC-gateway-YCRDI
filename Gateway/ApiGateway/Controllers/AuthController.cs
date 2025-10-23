using AuthService;
using Microsoft.AspNetCore.Mvc;
using static AuthService.AuthService;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthServiceClient grpcClient) : ControllerBase
{
    private readonly AuthServiceClient _grpcClient = grpcClient;

    [HttpPost("sign_in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        try
        {
            var response = await _grpcClient.SignInAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("sign_up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        try
        {
            var response = await _grpcClient.SignUpAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}