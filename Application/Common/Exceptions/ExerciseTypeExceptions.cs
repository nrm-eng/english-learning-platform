namespace Application.Common.Exceptions;

public class ExerciseTypeNotFoundException(int exerciseTypeId)
    : BaseException(exerciseTypeId, $"Exercise type not found under id {exerciseTypeId}");

public class ExerciseTypeAlreadyExistsException(string name)
    : BaseException(0, $"Exercise type with name {name} already exists");

public class ExerciseTypeHasExercisesException(int exerciseTypeId)
    : BaseException(exerciseTypeId, $"Exercise type with id {exerciseTypeId} has related exercises and cannot be deleted");

public class UnhandledExerciseTypeException(int exerciseTypeId, Exception? innerException = null)
    : BaseException(exerciseTypeId, "Unhandled exercise type exception", innerException);