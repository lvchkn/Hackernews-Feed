namespace Application.Ranking;

public interface IRankable
{
    int Time { get; }
    int Score { get; }
    double Rank { get; set; }
}