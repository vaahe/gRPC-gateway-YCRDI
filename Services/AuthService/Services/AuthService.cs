using AuthService.Interfaces;
using AuthService.Presentation;

namespace AuthService.Services
{
    public abstract class AuthServiceLogic(ILogger<AuthServiceImpl> logger) : IAuthService
    {
        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            logger.LogInformation("Processing SignUp");

            await Task.CompletedTask;

            return new SignUpResponse
            {
               Status = true
            };
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest request)
        {
            logger.LogInformation("Processing SignIn for user: {Username}", request.Username);

            await Task.CompletedTask;

            return new SignInResponse
            {
                UserId = Guid.NewGuid().ToString()
            };
        }
    }
}