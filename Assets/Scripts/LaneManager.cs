using UnityEngine;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    [Header("Lane Settings")]
    public float[] laneXPositions = { -2f, 0f, 2f };
    public float laneLength = 50f; // Lane uzunluðu
    public float laneWidth = 1.5f;

    [Header("Visual Settings")]
    public Material laneMaterial;
    public Color[] laneColors = { Color.red, Color.green, Color.blue };

    private List<GameObject> laneVisuals = new List<GameObject>();

    void Start()
    {
        CreateLaneVisuals();
    }

    void CreateLaneVisuals()
    {
        for (int i = 0; i < laneXPositions.Length; i++)
        {
            // Lane görseli oluþtur
            GameObject lane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lane.name = $"Lane_{i}";
            lane.transform.position = new Vector3(laneXPositions[i], 0.01f, laneLength / 2f);
            lane.transform.localScale = new Vector3(laneWidth, 0.02f, laneLength);

            // Material ve renk ayarla
            Renderer renderer = lane.GetComponent<Renderer>();
            if (laneMaterial != null)
            {
                renderer.material = laneMaterial;
            }
            if (i < laneColors.Length)
            {
                renderer.material.color = laneColors[i];
            }

            // Collider'ý kaldýr (sadece görsel)
            DestroyImmediate(lane.GetComponent<Collider>());

            laneVisuals.Add(lane);
        }
    }

    public Vector3 GetLaneEndPosition(int laneIndex)
    {
        if (laneIndex < 0 || laneIndex >= laneXPositions.Length)
            return Vector3.zero;

        return new Vector3(laneXPositions[laneIndex], 0.5f, laneLength);
    }

    public Vector3 GetRandomSpawnPosition()
    {
        int randomLane = Random.Range(0, laneXPositions.Length);
        return GetLaneEndPosition(randomLane);
    }
}