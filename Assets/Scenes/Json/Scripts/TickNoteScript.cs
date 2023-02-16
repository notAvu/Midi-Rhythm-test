using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickNoteScript : MonoBehaviour
{
     //TODO Get position and instantiation from lanes 
    public NoteObject noteData;
    public float InstantiationTimestamp;
    public float NoteTimestamp;
    private float TimeSienceInstantiation;
    public SpawnColumn Column;
    public RhythmConductor conductor { private get; set; } 
    private void Awake()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    private void Update()
    {
        TimeSienceInstantiation = conductor.songPositionSeconds - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        if (t > 1)
        {
            //Destroy(gameObject);
        }
        else if(conductor.lastBeat>= InstantiationTimestamp/conductor.secondsPerNote)
        {
            transform.position = Vector2.Lerp(Column.spawnPosition,Column.despawnPosition, t);
        }
    }
}
