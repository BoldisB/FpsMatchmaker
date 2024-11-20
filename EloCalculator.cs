namespace FpsMatchmaker
{
    public static class EloCalculator
    {
        public static double CalculateNewElo(double currentElo, double opponentAvgElo, double score, int k)
        {
            double expectedScore = 1 / (1 + Math.Pow(10, (opponentAvgElo - currentElo) / 400));
            return currentElo + k * (score - expectedScore);
        }

        public static int DetermineK(int hoursPlayed)
        {
            if (hoursPlayed < 500) return 50;
            if (hoursPlayed < 1000) return 40;
            if (hoursPlayed < 3000) return 30;
            if (hoursPlayed < 5000) return 20;
            return 10;
        }
    }
}
