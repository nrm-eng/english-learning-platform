namespace Application.Common.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException(int userId)
        : base(userId, $"User not found under id {userId}") { }

    public UserNotFoundException(string email)
        : base(0, $"User with email {email} was not found") { }
}

public class UserAlreadyExistsException : BaseException
{
    public UserAlreadyExistsException(string email)
        : base(0, $"User with email {email} already exists") { }
}

public class UnhandledUserException(int userId, Exception? innerException = null)
    : BaseException(userId, "Unhandled user exception", innerException);