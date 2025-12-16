using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public static TimerUI Instance;
    public TextMeshProUGUI timerText;

    private float time;
    private bool running = true;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!running) return;

        time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        running = false;
    }
}
