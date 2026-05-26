using System;

namespace Legacy.World
{
    [Serializable]
    public sealed class MiniGameResultState
    {
        public int Score { get; }
        public int MaxScore { get; }
        public int Quality { get; }
        public int DurationSeconds { get; }
        public int Mistakes { get; }

        public MiniGameResultState(int score, int maxScore, int quality, int durationSeconds, int mistakes)
        {
            Score = score;
            MaxScore = maxScore;
            Quality = quality;
            DurationSeconds = durationSeconds;
            Mistakes = mistakes;
        }
    }
}
