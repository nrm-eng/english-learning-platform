using Application.Common.Exceptions;

public class LevelNotFoundException(int levelId)
    : BaseException(levelId, $"Level not found under id {levelId}");