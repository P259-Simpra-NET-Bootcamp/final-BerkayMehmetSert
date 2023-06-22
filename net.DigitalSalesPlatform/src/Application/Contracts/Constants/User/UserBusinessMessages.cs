namespace Application.Contracts.Constants.User;

public static class UserBusinessMessages
{
    public const string UserNotFoundByEmail = "User with the provided email address was not found.";
    public const string InvalidPassword = "Invalid password. Please check your password and try again.";

    public const string UserAlreadyExistByEmail = "This email is already associated with an existing user account." +
                                                  " Please use a different email address or log in with your existing account.";

    public const string UserNotFoundById = "User with the specified ID does not exist.";
    public const string UserIsNotActive = "The user is not active.";
    
    public const string PasswordNotMatchWithOldPassword ="Password does not match with the old password.";
    public const string PasswordNotMatchWithConfirmPassword = "The password does not match with the confirm password.";
}