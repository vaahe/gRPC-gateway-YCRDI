using Database;
using Grpc.Core;
using System.Text;
using Database.Models;
using FluentValidation;
using AuthService.Validators;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using AuthService.Application.Interfaces;

namespace AuthService.Application.Services
{
    public class AuthServiceImpl(DatabaseContext context, ILogger logger) : IAuthService
    {
        private readonly SignUpRequestValidation _signUpRequestValidation = new();
        private readonly SignInRequestValidation _signInRequestValidation = new();

        public async Task<SignInResponse> SignInAsync(SignInRequest request)
        {
            ValidateRequest(_signInRequestValidation, request);

            try
            {
                logger.LogInformation("SignIn attempt for user: {Username}", request.Username);
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (user == null)
                {
                    return new SignInResponse { UserId = "0" };
                }

                var isPasswordValid = VerifyPassword(request.Password, user.PasswordHash);
                return !isPasswordValid ? new SignInResponse { UserId = "0" } : new SignInResponse { UserId = user.Id.ToString() };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while logging in user: {Username}", request.Username);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            ValidateRequest(_signUpRequestValidation, request);

            try
            {
                logger.LogInformation("SignUp attempt for user: {Username}", request.Username);
                
                var exists = await context.Users.AnyAsync(u => u.Username == request.Username);
                if (exists)
                {
                    return new SignUpResponse { Status = false };
                }

                var hashedPassword = HashPassword(request.Password);
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = hashedPassword,
                };

                context.Users.Add(newUser);
                await context.SaveChangesAsync();

                return new SignUpResponse { Status = true };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while register");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }

        // ===== helper methods =====
        private static string HashPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var hashOfPassword = HashPassword(password);
            return hashOfPassword == storedHash;
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
