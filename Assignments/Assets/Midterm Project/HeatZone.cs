using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeatZone : MonoBehaviour
{
    private LineRenderer[] lines;

    [Range(0f, 1f)]
    public float coolProgress = 0f;  
    public float coolDuration = 3f; 
    private bool isFullyCooled = false;

    private Color[] originalColors;

    private List<Vector2> polygonVerts2D = new List<Vector2>();

    public static List<HeatZone> AllZones = new List<HeatZone>();

    private void Awake()
    {
        AllZones.Add(this);
    }

    private void OnDestroy()
    {
        AllZones.Remove(this);
    }

    private void Start()
    {
        lines = GetComponentsInChildren<LineRenderer>();

        originalColors = new Color[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] != null && lines[i].material != null)
                originalColors[i] = lines[i].material.color;
            else
                originalColors[i] = Color.red; 
        }
    }

    public void SetPolygonVertices(Vector3[] worldVerts)
    {
        polygonVerts2D.Clear();
        foreach (var v in worldVerts)
            polygonVerts2D.Add(new Vector2(v.x, v.z));
    }

    public bool ContainsPoint(Vector3 worldPos)
    {
        if (polygonVerts2D.Count < 3) return false;

        Vector2 p = new Vector2(worldPos.x, worldPos.z);
        bool inside = false;

        for (int i = 0, j = polygonVerts2D.Count - 1; i < polygonVerts2D.Count; j = i++)
        {
            Vector2 vi = polygonVerts2D[i];
            Vector2 vj = polygonVerts2D[j];

            bool intersect = ((vi.y > p.y) != (vj.y > p.y)) &&
                             (p.x < (vj.x - vi.x) * (p.y - vi.y) / (vj.y - vi.y) + vi.x);
            if (intersect)
                inside = !inside;
        }
        return inside;
    }

    public void CoolDownStep(float deltaTime)
    {
        if (isFullyCooled) return;

        // increase progress gradually while holding space
        coolProgress += deltaTime / coolDuration;
        coolProgress = Mathf.Clamp01(coolProgress);

        UpdateZoneColor();

        if (coolProgress >= 1f)
        {
            isFullyCooled = true;
            Debug.Log($"{gameObject.name} fully cooled!");
            MidTermGameManager.Instance.RegisterZoneHealed();
        }
    }
    private void UpdateZoneColor()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == null || lines[i].material == null)
                continue;

            Color startColor = (i < originalColors.Length) ? originalColors[i] : Color.red;
            Color targetColor = Color.Lerp(startColor, Color.green, coolProgress);
            lines[i].material.color = targetColor;
        }
    }

}
