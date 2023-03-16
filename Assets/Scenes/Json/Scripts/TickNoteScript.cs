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
    public LongNote HeadNote;
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
            if (transform.position.y <= HeadNote.transform.position.y || transform.position.y>= Column.HitBar.position.y)
            {
                Destroy(gameObject);
            }
        }
        else if (conductor.lastBeat >= InstantiationTimestamp / conductor.secondsPerNote)
        {
            if (HeadNote.BeingHit && transform.position.y <= Column.HitBar.transform.position.y)
            {
                var target = new Vector2(transform.position.x, Column.HitBar.position.y);
            }
            else
            {
                transform.position = Vector2.Lerp(Column.spawnPosition, Column.despawnPosition, t);
            }
        }
    }
}
