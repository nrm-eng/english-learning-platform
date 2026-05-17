namespace Application.Common.Exceptions;

public class QuestionNotFoundException(int questionId)
    : BaseException(questionId, $"Question not found under id {questionId}");

public class UnhandledQuestionException(int questionId, Exception? innerException = null)
    : BaseException(questionId, "Unhandled question exception", innerException);