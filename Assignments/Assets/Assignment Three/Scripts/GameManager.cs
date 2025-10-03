using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float baseTime = 20f;
    public int totalCollectables = 5;
    private int collectedCount = 0;
    private float timer;

    public CarController playerCar;
    public TextMeshProUGUI timerText;

    public GameObject WinLoseObject;
    public TextMeshProUGUI winLoseText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timer = baseTime;
    }

    void Update()
    {
        if (playerCar == null) return;

        timer -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timer);
        }

        if (timer <= 0f)
        {
            Debug.Log("Time's up! You Lose!");
            WinLoseObject.SetActive(true);
            winLoseText.text = "You Lose!";
            playerCar.enabled = false;
            enabled = false;

        }

        if (collectedCount >= totalCollectables)
        {
            Debug.Log("All collectables gathered! You Win!");
            WinLoseObject.SetActive(true);
            winLoseText.text = "You Win!";
            playerCar.enabled = false;
            enabled = false;
        }
    }

    public void CollectCollectable(float timeBonus)
    {
        collectedCount++;
        timer += timeBonus;
        Debug.Log("Collected! Total: " + collectedCount + "/" + totalCollectables + " | Timer +" + timeBonus);
    }
}

