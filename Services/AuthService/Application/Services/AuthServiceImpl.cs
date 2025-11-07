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
    public class AuthServiceImpl : IAuthService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<AuthServiceImpl> _logger;
        
        private readonly SignUpRequestValidation _signUpRequestValidation = new();
        private readonly SignInRequestValidation _signInRequestValidation = new();
        
        public AuthServiceImpl(DatabaseContext context, ILogger<AuthServiceImpl> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest request)
        {
            ValidateRequest(_signInRequestValidation, request);

            try
            {
                _logger.LogInformation("SignIn attempt for user: {Username}", request.Username);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (user == null)
                {
                    return new SignInResponse { UserId = "0" };
                }

                var isPasswordValid = VerifyPassword(request.Password, user.PasswordHash);
                return !isPasswordValid ? new SignInResponse { UserId = "0" } : new SignInResponse { UserId = user.Id.ToString() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while logging in user: {Username}", request.Username);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            ValidateRequest(_signUpRequestValidation, request);

            try
            {
                _logger.LogInformation("SignUp attempt for user: {Username}", request.Username);
                
                var exists = await _context.Users.AnyAsync(u => u.Username == request.Username);
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

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return new SignUpResponse { Status = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while register");
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
            => HashPassword(password) == storedHash;

        private static void ValidateRequest<T>(IValidator<T> validator, T request)
        {
            var validationResult = validator.Validate(request);

            if (validationResult.IsValid) return;
            
            var message = validationResult.Errors.Select(e => e.ErrorMessage);
            throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", message)));
        }
    }
}
