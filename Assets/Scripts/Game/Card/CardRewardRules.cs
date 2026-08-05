public static class CardRewardRules
{
    public const int InitialMaximumManaCost = 3;
    public const int ManaCostUnlockIntervalRounds = 3;
    public const int MaximumProgressionSteps = 3;

    public static int GetMaximumRewardManaCost(int completedRounds)
    {
        int normalizedRounds = completedRounds < 1 ? 1 : completedRounds;
        int progressionStep = (normalizedRounds - 1) / ManaCostUnlockIntervalRounds;
        if (progressionStep > MaximumProgressionSteps)
        {
            progressionStep = MaximumProgressionSteps;
        }

        return InitialMaximumManaCost + progressionStep;
    }

    public static bool IsTransformationProgressionAvailable(int cardManaCost, int minimumTransformationCost)
    {
        return minimumTransformationCost != int.MaxValue && cardManaCost == minimumTransformationCost;
    }
}
