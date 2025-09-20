using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RisingSeaLevel : MonoBehaviour
{
    [SerializeField] private Slider seaLevelSlider;
    [SerializeField] private float minHeight = 0f;   // starting sea level
    [SerializeField] private float maxHeight = 50f;  // max sea level rise

    [Header("UI Display")]
    [SerializeField] private TextMeshProUGUI seaLevelText; // use this if TMP
    // [SerializeField] private Text seaLevelText; // uncomment if using legacy Text

    private void Start()
    {
        if (seaLevelSlider != null)
        {
            seaLevelSlider.minValue = minHeight;
            seaLevelSlider.maxValue = maxHeight;
            seaLevelSlider.value = minHeight;

            UpdateSeaLevelText(seaLevelSlider.value);
        }
    }

    private void Update()
    {
        if (seaLevelSlider != null)
        {
            // Update water height
            Vector3 pos = transform.position;
            pos.y = seaLevelSlider.value;
            transform.position = pos;

            // Update text
            UpdateSeaLevelText(seaLevelSlider.value);
        }
    }

    private void UpdateSeaLevelText(float value)
    {
        if (seaLevelText != null)
        {
            seaLevelText.text = $"Sea Level: {value:0.0} m";
        }
    }
}
