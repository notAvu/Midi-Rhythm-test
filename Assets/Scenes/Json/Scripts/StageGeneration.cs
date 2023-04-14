using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGeneration : MonoBehaviour
{
    private RhythmConductor conductor;
    [SerializeField]
    private GameObject lanePrefab;
    [SerializeField]
    private float horizontalSize;
    [SerializeField]
    private float yPosition;
    [SerializeField]
    private float margin;
    private int laneCount;
    private float laneDistance;
    private void Start()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
        //Camera cam = Camera.main;
        //horizontalSize = cam.scaledPixelWidth; // TODO convert to units
        laneCount = conductor.columnCount;
        laneDistance = (horizontalSize - margin * 2 + lanePrefab.transform.localScale.x) / laneCount;
        GenerateLanes();
    }
    public void GenerateLanes()
    {
        float firstLanePosition = (horizontalSize / 2) - margin;
        for (int i = 1; i <= laneCount; i++)
        {
            _ = Instantiate(lanePrefab, new Vector2(firstLanePosition+(laneDistance * laneCount), yPosition), Quaternion.identity);
        }
    }
}
