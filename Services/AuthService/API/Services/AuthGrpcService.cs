using AuthService.Application.Interfaces;
using Grpc.Core;

namespace AuthService.API.Services
{
    public class AuthGrpcService(IAuthService authService) : AuthService.AuthServiceBase
    {
        private readonly IAuthService _authService = authService;

        public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
            => await _authService.SignInAsync(request);

        public override async Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
            => await _authService.SignUpAsync(request);
    }
}