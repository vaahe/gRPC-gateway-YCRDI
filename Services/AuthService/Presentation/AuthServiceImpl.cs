using Grpc.Core;
using FluentValidation;
using AuthService.Services;
using AuthService.Validators;

namespace AuthService.Presentation
{
    public class AuthServiceImpl(
        AuthServiceLogic authService,
        ILogger<AuthServiceImpl> logger,
        IValidator<SignUpRequest> signUpValidator,
        IValidator<SignInRequest> signInValidator)
        : AuthService.AuthServiceBase
    {
        public override async Task<SignInResponse> SignIn(SignInRequest request, ServerCallContext context)
        {
            ValidateRequest<SignInRequest>(signInValidator, request);

            try
            {
                logger.LogInformation("SignIn attempt for user: {Username}", request.Username);
                return await authService.SignInAsync(request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while logging in user: {Username}", request.Username);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
        public override async Task<SignUpResponse> SignUp(SignUpRequest request, ServerCallContext context)
        {
            ValidateRequest(signUpValidator, request);
            
            try
            {
                logger.LogInformation("SignUp attempt for user: {Username}", request.Username);
                return await authService.SignUpAsync(request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while register");
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        private static void ValidateRequest<T>(IValidator<T> validator, T request)
        {
            var validationResult = validator.Validate(request);

            if (validationResult.IsValid) return;
            
            var message = validationResult.Errors.Select(e => e.ErrorMessage);
            throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", message)));
        }
    }
}
