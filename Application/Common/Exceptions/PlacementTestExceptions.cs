namespace Application.Common.Exceptions;

public class PlacementTestNotFoundException(int userId)
    : BaseException(userId, $"Placement test not found for user with id {userId}");