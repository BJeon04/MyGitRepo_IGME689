using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MidTermGameManager : MonoBehaviour
{
    public static bool GameOver = false;

    public TextMeshProUGUI timerText;  
    public TextMeshProUGUI progressText;

    public GameObject endScreen;
    public TextMeshProUGUI finalTimeText;
    public Button mainMenuButton;

    private float elapsedTime = 0f;
    private bool timerRunning = false;

    private int currentScore = 0;
    private int targetScore = 0;

    private DroneController drone;

    public static MidTermGameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    private void Start()
    {
        targetScore = DifficultyManager.GetTargetZoneCount();
        Debug.Log("Difficulty: " + DifficultyManager.SelectedDifficulty);
        Debug.Log("Zones required: " + targetScore);

        StartTimer();
        UpdateProgressUI();

        if (endScreen != null)
            endScreen.SetActive(false);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        drone = FindFirstObjectByType<DroneController>();

    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RegisterZoneHealed()
    {
        currentScore++;
        UpdateProgressUI();

        if (currentScore % 5 == 0)
        {
            drone.IncreaseStats();
        }

        if (currentScore >= targetScore)
        {
            StopTimer();
            Debug.Log($"All zones healed in {elapsedTime:F1} seconds!");
            ShowEndScreen();
        }
    }

    private void UpdateProgressUI()
    {
        if (progressText != null)
            progressText.text = $"{currentScore} / {targetScore} Zones Cooled";
    }

    private void ShowEndScreen()
    {
        if (endScreen != null)
        {
            endScreen.SetActive(true);

            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            finalTimeText.text = $"You finished in {minutes:00}:{seconds:00}";
            GameOver = true;
            Debug.Log("You Win! Game Complete!");
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
