using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ErrorFactory
{
    public static ObjectResult ToObjectResult(this BaseException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                UserNotFoundException => StatusCodes.Status404NotFound,
                UserAlreadyExistsException => StatusCodes.Status409Conflict,
                UnhandledUserException => StatusCodes.Status500InternalServerError,

                RoleNotFoundException => StatusCodes.Status404NotFound,
                RoleAlreadyExistsException => StatusCodes.Status409Conflict,
                RoleHasUsersException => StatusCodes.Status409Conflict,
                UnhandledRoleException => StatusCodes.Status500InternalServerError,

                LevelNotFoundException => StatusCodes.Status404NotFound,

                ExerciseTypeNotFoundException => StatusCodes.Status404NotFound,
                ExerciseTypeAlreadyExistsException => StatusCodes.Status409Conflict,
                ExerciseTypeHasExercisesException => StatusCodes.Status409Conflict,
                UnhandledExerciseTypeException => StatusCodes.Status500InternalServerError,

                ExerciseNotFoundException => StatusCodes.Status404NotFound,
                UnhandledExerciseException => StatusCodes.Status500InternalServerError,

                QuestionNotFoundException => StatusCodes.Status404NotFound,
                UnhandledQuestionException => StatusCodes.Status500InternalServerError,

                AnswerOptionNotFoundException => StatusCodes.Status404NotFound,
                UnhandledAnswerOptionException => StatusCodes.Status500InternalServerError,

                PlacementTestNotFoundException => StatusCodes.Status404NotFound,
                UnhandledPlacementTestException => StatusCodes.Status500InternalServerError,

                UserExerciseResultNotFoundException => StatusCodes.Status404NotFound,
                UnhandledUserExerciseResultException => StatusCodes.Status500InternalServerError,

                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}