using UnityEngine;

public class PlatformGun : MonoBehaviour
{
    public GameObject platformPrefab;
    public float fireCooldown = 0.3f;

    private float lastFireTime;
    private GunManager gunManager;

    void Start()
    {
        gunManager = GetComponent<GunManager>();
    }

    void Update()
    {
        if (!gunManager.IsPlatformActive()) return;


        if (Input.GetMouseButtonDown(1))
        {
            TryFire();
        }
    }

    void TryFire()
    {
        if (Time.time < lastFireTime + fireCooldown) return;

        Camera cam = Camera.main;

        GameObject platform = Instantiate(
            platformPrefab,
            cam.transform.position + cam.transform.forward * 1f,
            cam.transform.rotation
        );

        lastFireTime = Time.time;
    }
}
