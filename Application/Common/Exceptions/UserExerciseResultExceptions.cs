namespace Application.Common.Exceptions;

public class UserExerciseResultNotFoundException(int userId, int exerciseId)
    : BaseException(userId, $"Exercise result not found for user {userId} and exercise {exerciseId}");