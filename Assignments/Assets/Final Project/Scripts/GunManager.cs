using UnityEngine;

public class GunManager : MonoBehaviour
{
    public enum GunType
    {
        Grapple,
        Platform
    }

    public GunType currentGun = GunType.Grapple;
    public static GunManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentGun = currentGun == GunType.Grapple
                ? GunType.Platform
                : GunType.Grapple;

            Debug.Log("Switched to: " + currentGun);
        }
    }

    public bool IsGrappleActive()
    {
        return currentGun == GunType.Grapple;
    }

    public bool IsPlatformActive()
    {
        return currentGun == GunType.Platform;
    }
}
