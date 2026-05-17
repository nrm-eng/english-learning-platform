namespace Application.Common.Exceptions;

public class AnswerOptionNotFoundException(int answerOptionId)
    : BaseException(answerOptionId, $"Answer option not found under id {answerOptionId}");

public class UnhandledAnswerOptionException(int answerOptionId, Exception? innerException = null)
    : BaseException(answerOptionId, "Unhandled answer option exception", innerException);

public class AnswerOptionAlreadyExistsException(int questionId)
    : BaseException(questionId, $"Answer option already exists for question {questionId}");