using Microsoft.AspNetCore.Identity;

namespace TinyCRM.API.Exceptions;

public class IdentityException : HttpException
{
    public IdentityException(IdentityError error) : base(error.Description)
    {
        StatusCode = GetStatusCode(error.Code);
        ErrorCode = error.Code;
    }

    private static int GetStatusCode(string errorCode)
    {
        switch (errorCode)
        {
            case nameof(IdentityErrorDescriber.DuplicateEmail):
            case nameof(IdentityErrorDescriber.DuplicateUserName):
            case nameof(IdentityErrorDescriber.DuplicateRoleName):
            case nameof(IdentityErrorDescriber.UserAlreadyInRole):
                return StatusCodes.Status409Conflict;

            case nameof(IdentityErrorDescriber.LoginAlreadyAssociated):
            case nameof(IdentityErrorDescriber.PasswordMismatch):
            case nameof(IdentityErrorDescriber.InvalidToken):
            case nameof(IdentityErrorDescriber.InvalidRoleName):
            case nameof(IdentityErrorDescriber.InvalidUserName):
            case nameof(IdentityErrorDescriber.InvalidEmail):
            case nameof(IdentityErrorDescriber.UserLockoutNotEnabled):
            case nameof(IdentityErrorDescriber.UserAlreadyHasPassword):
            case nameof(IdentityErrorDescriber.PasswordTooShort):
            case nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric):
            case nameof(IdentityErrorDescriber.PasswordRequiresDigit):
            case nameof(IdentityErrorDescriber.PasswordRequiresLower):
            case nameof(IdentityErrorDescriber.PasswordRequiresUpper):
            case nameof(IdentityErrorDescriber.ConcurrencyFailure):
            case nameof(IdentityErrorDescriber.DefaultError):
                return StatusCodes.Status400BadRequest;

            case nameof(IdentityErrorDescriber.UserNotInRole):
                return StatusCodes.Status404NotFound;
        }

        return StatusCodes.Status500InternalServerError;
    }
}