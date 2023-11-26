using Godot;
using static System.Formats.Asn1.AsnWriter;

public partial class ScoreManager : Node
{
    private long _score;
    public long Score { get => _score; }

    private int _hitStreak;
    public int HitStreak { get => _hitStreak; }

    [Export]
    private ScoreDisplay scoreDisplay;

    [Export]
    private int okValue = 50;
    [Export]
    private int perfectValue = 100;
    [Export]
    private int hitStreakMax = 8;

    public int HitNote(HitType hitType)
    {
        int addedValue = 0;

        switch (hitType)
        {
            case HitType.MISSED:
                addedValue = 0;
                _hitStreak = 0;
                break;
            case HitType.OK:
                AddHitStreak();
                addedValue = okValue * _hitStreak;
                _score += addedValue;
                break;
            case HitType.PERFECT:
                AddHitStreak();
                addedValue = perfectValue * _hitStreak;
                _score += addedValue;
                break;
        }

        scoreDisplay.DisplayScore(_score);

        return addedValue;
    }

    private void AddHitStreak()
    {
        if (_hitStreak < hitStreakMax)
        {
            _hitStreak++;
        }
    }

}
