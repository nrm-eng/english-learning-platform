namespace Application.Common.Exceptions;

public class ExerciseNotFoundException(int exerciseId)
    : BaseException(exerciseId, $"Exercise not found under id {exerciseId}");

public class UnhandledExerciseException(int exerciseId, Exception? innerException = null)
    : BaseException(exerciseId, "Unhandled exercise exception", innerException);