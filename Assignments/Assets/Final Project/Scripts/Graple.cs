using UnityEngine;

[RequireComponent(typeof(FPSController))]
public class Grapple : MonoBehaviour
{
    public float maxGrappleDistance = 30f;
    public float grappleSpeed = 15f;
    public LineRenderer line; 
    public LayerMask grappleLayer; 

    private bool isGrappling = false;
    private Vector3 grapplePoint;
    private FPSController fpsController;

    [Header("Crosshair")]
    public UnityEngine.UI.Image crosshair;
    public Color normalColor = Color.white;
    public Color grappleColor = Color.green;

    [Header("Momentum")]
    public float momentumDamping = 0.95f;

    private Vector3 storedMomentum;

    [Header("Grapple Jump")]
    public float grappleJumpForce = 12f;

    private GunManager gunManager;


    void Start()
    {
        fpsController = GetComponent<FPSController>();
        gunManager = GetComponent<GunManager>();

        if (line != null)
        {
            line.positionCount = 0;
        }
    }

    void Update()
    {
        UpdateCrosshair();
        HandleGrappleInput();
        HandleGrappleMovement();
        ApplyMomentum();
    }

    void HandleGrappleInput()
    {
        if (!gunManager.IsGrappleActive()) return;

        if (Input.GetMouseButtonDown(1)) // Right click to grapple
        {
            TryGrapple();
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }
    }

   
    void TryGrapple()
    {
        Camera cam = Camera.main;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance))
        {
            if (IsEsriBuildingMesh(hit.collider.gameObject))
            {
                grapplePoint = hit.point;
                StartGrapple();
            }
        }
    }

    void StartGrapple()
    {
        fpsController.ResetVerticalVelocity();

        isGrappling = true;

        if (line != null)
        {
            line.positionCount = 2;
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, grapplePoint);
        }
    }


    void StopGrapple()
    {
        isGrappling = false;
        if (line != null)
        {
            line.enabled = false;
            line.positionCount = 0;

        }
    }

    void ApplyMomentum()
    {
        if (isGrappling) return;

        if (storedMomentum.magnitude > 0.1f)
        {
            fpsController.controller.Move(storedMomentum * Time.deltaTime);
            storedMomentum *= momentumDamping;
        }
    }


    void HandleGrappleMovement()
    {
        if (!isGrappling) return;

        Vector3 dir = (grapplePoint - transform.position).normalized;
        storedMomentum = dir * grappleSpeed;

        fpsController.controller.Move(dir * grappleSpeed * Time.deltaTime);

        fpsController.ResetVerticalVelocity();

        if (Vector3.Distance(transform.position, grapplePoint) < 1.5f)
        {
            StopGrapple();
            return;
        }

        if (line != null && line.positionCount >= 2)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, grapplePoint);
        }

        if (isGrappling && Input.GetKeyDown(KeyCode.Space))
        {
            GrappleJump();
        }

    }
    bool IsEsriBuildingMesh(GameObject obj)
    {
        var map = FindFirstObjectByType<Esri.ArcGISMapsSDK.Components.ArcGISMapComponent>();
        if (map == null) return false;

        return obj.transform.IsChildOf(map.transform);
    }

    void UpdateCrosshair()
    {
        if (crosshair == null) return;

        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance))
        {
            if (IsEsriBuildingMesh(hit.collider.gameObject))
            {
                crosshair.color = grappleColor;
                return;
            }
        }

        crosshair.color = normalColor;
    }

    void GrappleJump()
    {
        Vector3 forwardDir = (grapplePoint - transform.position).normalized;

        Vector3 launchDir = (forwardDir * 1.2f + Vector3.up * 0.6f).normalized;

        StopGrapple();

        fpsController.AddMomentum(launchDir * grappleJumpForce);

        fpsController.AddBoost(grappleJumpForce * 0.5f);
    }

}
