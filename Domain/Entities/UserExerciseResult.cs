using Domain.Interfaces;

namespace Domain.Entities;

public class UserExerciseResult : IEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int ExerciseId { get; private set; }
    public int Score { get; private set; }
    public int MaxScore { get; private set; }
    public DateTime CompletedAt { get; private set; }

    public User? User { get; private set; }
    public Exercise? Exercise { get; private set; }

    private UserExerciseResult(int userId, int exerciseId,
        int score, int maxScore, DateTime completedAt)
    {
        UserId = userId;
        ExerciseId = exerciseId;
        Score = score;
        MaxScore = maxScore;
        CompletedAt = completedAt;
    }

    public static UserExerciseResult New(int userId, int exerciseId, int score, int maxScore)
        => new UserExerciseResult(userId, exerciseId, score, maxScore, DateTime.UtcNow);

    public void UpdateResult(int score, int maxScore)
    {
        Score = score;
        MaxScore = maxScore;
        CompletedAt = DateTime.UtcNow;
    }
}