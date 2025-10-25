using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuUI : MonoBehaviour
{
    public void OnEasyClicked()
    {
        DifficultyManager.SetDifficulty(DifficultyManager.Difficulty.Easy);
        SceneManager.LoadScene("Midterm Project");
    }

    public void OnMediumClicked()
    {
        DifficultyManager.SetDifficulty(DifficultyManager.Difficulty.Medium);
        SceneManager.LoadScene("Midterm Project");
    }

    public void OnHardClicked()
    {
        DifficultyManager.SetDifficulty(DifficultyManager.Difficulty.Hard);
        SceneManager.LoadScene("Midterm Project");
    }
}
