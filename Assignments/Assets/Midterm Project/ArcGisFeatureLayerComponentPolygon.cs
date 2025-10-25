using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
public class ArcGisFeatureLayerComponentPolygon : MonoBehaviour
{
    [System.Serializable]
    public struct QueryLink
    {
        public string Link;
        public string[] RequestHeaders;
    }

    private ArcGISMapComponent mapComponent;

    [Header("Feature Layer Setup")]
    public QueryLink WebLink;
    public Material lineMaterial;
    public float lineWidth = 10f;

    private void Start()
    {
        mapComponent = FindFirstObjectByType<ArcGISMapComponent>();
        StartCoroutine(GetFeatures());
    }

    private IEnumerator GetFeatures()
    {
        UnityWebRequest request = UnityWebRequest.Get(WebLink.Link);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Feature request failed: {request.error}");
            yield break;
        }

        string json = request.downloadHandler.text;
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("No JSON text received from server.");
            yield break;
        }

        Debug.Log("Received JSON from server!");
        CreateGameObjectsFromResponse(json);
    }

    private void CreateGameObjectsFromResponse(string response)
    {
        var jObject = JObject.Parse(response);
        var features = jObject.SelectToken("features")?.ToArray();
        if (features == null || features.Length == 0)
        {
            Debug.LogWarning("No features found in dataset.");
            return;
        }

        Debug.Log($"Loaded {features.Length} features from dataset.");

        foreach (var feature in features)
        {
            var geom = feature.SelectToken("geometry");
            if (geom == null || geom["rings"] == null)
                continue;

            var hviToken = feature.SelectToken("attributes.HVI");
            float hviValue = 0;
            if (hviToken != null)
                float.TryParse(hviToken.ToString(), out hviValue);

            CreatePolygonOutline(geom["rings"], hviValue);
        }
    }

    private void CreatePolygonOutline(JToken ringsToken, float hvi)
    {
        var rings = ringsToken.ToArray();

        GameObject zoneObj = new GameObject("HeatZone");
        zoneObj.tag = "HeatZone";
        var heatZone = zoneObj.AddComponent<HeatZone>();

        Vector3 totalCenter = Vector3.zero;
        int totalPoints = 0;
        List<Vector3> firstRingVerts = new List<Vector3>();

        foreach (var ring in rings)
        {
            var ringCoords = ring.ToArray();
            if (ringCoords.Length < 3) continue;

            GameObject lineObj = new GameObject("PolygonOutline");
            lineObj.transform.SetParent(zoneObj.transform);

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            Material coloredMat = new Material(lineMaterial);
            Color c = GetColorFromHVI(hvi);
            coloredMat.color = c;
            lr.material = coloredMat;
            lr.widthMultiplier = lineWidth;
            lr.loop = true;

            Vector3[] positions = new Vector3[ringCoords.Length];
            for (int i = 0; i < ringCoords.Length; i++)
            {
                double lon = (double)ringCoords[i][0];
                double lat = (double)ringCoords[i][1];

                ArcGISPoint arcPoint = new ArcGISPoint(lon, lat, 0, new ArcGISSpatialReference(4326));
                Vector3 pos = mapComponent.GeographicToEngine(arcPoint);
                positions[i] = pos;

                if (firstRingVerts.Count < 2000)
                    firstRingVerts.Add(pos);

                totalCenter += pos;
                totalPoints++;
            }

            lr.positionCount = positions.Length;
            lr.SetPositions(positions);
        }

        if (totalPoints > 0)
            zoneObj.transform.position = totalCenter / totalPoints;

        heatZone.SetPolygonVertices(firstRingVerts.ToArray());

        Debug.Log("Heat zone created.");
    }

    private Color GetColorFromHVI(float hvi)
    {
        switch ((int)hvi)
        {
            case 1: return Color.green;
            case 2: return Color.yellow;
            case 3: return new Color(1f, 0.65f, 0f);
            case 4: return new Color(1f, 0.3f, 0f);
            case 5: return Color.red;
            default: return Color.gray;
        }
    }
}
