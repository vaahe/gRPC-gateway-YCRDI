namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<SignInResponse> SignInAsync(SignInRequest request);
        Task<SignUpResponse> SignUpAsync(SignUpRequest request);
    }
}