using Domain.Interfaces;

namespace Domain.Entities;

public class PlacementTest : IEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int LevelId { get; private set; }
    public int Score { get; private set; }
    public int MaxScore { get; private set; }
    public DateTime TakenAt { get; private set; }

    public User? User { get; private set; }
    public Level? Level { get; private set; }

    private PlacementTest(int userId, int levelId,
        int score, int maxScore, DateTime takenAt)
    {
        UserId = userId;
        LevelId = levelId;
        Score = score;
        MaxScore = maxScore;
        TakenAt = takenAt;
    }

    public static PlacementTest New(int userId, int levelId, int score, int maxScore)
        => new PlacementTest(userId, levelId, score, maxScore, DateTime.UtcNow);

    public void UpdateResult(int levelId, int score, int maxScore)
    {
        LevelId = levelId;
        Score = score;
        MaxScore = maxScore;
        TakenAt = DateTime.UtcNow;
    }
}