using UnityEngine;
using UnityEngine.UI;
using static GunManager;

public class GunUIIcons : MonoBehaviour
{
    public Image grappleIcon;
    public Image platformIcon;

    [Header("Visual Settings")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.35f);
    public float activeScale = 1.15f;
    public float inactiveScale = 1f;

    void Update()
    {
        var gun = GunManager.Instance.currentGun;

        SetIconState(grappleIcon, gun == GunType.Grapple);
        SetIconState(platformIcon, gun == GunType.Platform);
    }

    void SetIconState(Image icon, bool active)
    {
        icon.color = active ? activeColor : inactiveColor;
        icon.transform.localScale = active
            ? Vector3.one * activeScale
            : Vector3.one * inactiveScale;
    }
}
