using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }

    public static Difficulty SelectedDifficulty { get; private set; } = Difficulty.Easy;

    public static void SetDifficulty(Difficulty difficulty)
    {
        SelectedDifficulty = difficulty;
    }

    public static int GetTargetZoneCount()
    {
        switch (SelectedDifficulty)
        {
            case Difficulty.Easy:
                return 20;
            case Difficulty.Medium:
                return 35;
            case Difficulty.Hard:
                return 50;
            default:
                return 20;
        }
    }
}
