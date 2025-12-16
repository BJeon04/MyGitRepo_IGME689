using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;

    public int totalCollectibles = 5;
    private int collected = 0;
    private bool gameOver = false;

    void Awake()
    {
        Instance = this;
    }

    public void CollectItem()
    {
        collected++;

        if (collected >= totalCollectibles)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameOver = true;
        TimerUI.Instance.StopTimer();
        UIManager.Instance.ShowWinScreen();

        FPSController player = FindFirstObjectByType<FPSController>();
        if (player != null)
            player.EnableControls(false);
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
}
