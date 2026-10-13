namespace SnakeGame
{
    public static class GameSettings
    {
        // 0 = Easy, 1 = Medium, 2 = Hard
        public static int Difficulty = 1;

        public static int GetSnakeInterval()
        {
            switch (Difficulty)
            {
                case 0: return 150; // Easy
                case 1: return 100; // Medium
                case 2: return 60;  // Hard
                default: return 100;
            }
        }

        public static string GetDifficultyName()
        {
            switch (Difficulty)
            {
                case 0: return "Easy";
                case 1: return "Medium";
                case 2: return "Hard";
                default: return "Medium";
            }
        }
    }
}