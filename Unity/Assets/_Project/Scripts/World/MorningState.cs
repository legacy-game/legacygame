using System;
using Legacy.Time;

namespace Legacy.World
{
    [Serializable]
    public sealed class MorningState
    {
        public MorningStatus Status { get; private set; }
        public GameDateTime StartedAt { get; private set; }
        public GameDateTime EndedAt { get; private set; }
        public int TasksCompleted { get; private set; }
        public int MoneyEarnedCents { get; private set; }

        public MorningState(MorningStatus status, GameDateTime startedAt, GameDateTime endedAt, int tasksCompleted, int moneyEarnedCents)
        {
            Status = status;
            StartedAt = startedAt;
            EndedAt = endedAt;
            TasksCompleted = tasksCompleted;
            MoneyEarnedCents = moneyEarnedCents;
        }

        public void Start(GameDateTime startedAt)
        {
            Status = MorningStatus.Active;
            StartedAt = startedAt;
        }

        public void AddCompletedTask(int earnedCents)
        {
            TasksCompleted++;
            MoneyEarnedCents += earnedCents;
        }

        public void Complete(GameDateTime endedAt)
        {
            Status = MorningStatus.Complete;
            EndedAt = endedAt;
        }
    }
}
