using Grpc.Core;

namespace AuthService.Services
{
    public class AuthServiceImpl : AuthService.AuthServiceBase
    {
        public override Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SignInResponse
            {
                UserId = Guid.NewGuid().ToString(),
            });
        }
    public override Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SignUpResponse
            {
                Status = true,
            });
        }
    }
}
