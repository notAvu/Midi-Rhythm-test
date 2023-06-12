using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickNoteScript : HitNote
{
    //TODO Get position and instantiation from lanes 
    //public NoteObject noteData;
    public double InstantiationTimestamp;
    //public float NoteTimestamp;
    private double TimeSienceInstantiation;
    public SpawnColumn Column;
    //public RhythmConductor conductor { private get; set; }
    public LongNote HeadNote;
    private void Awake()
    {
        //conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    private void Update()
    {
        TimeSienceInstantiation = RhythmConductor.Instance.songPositionSeconds - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        if (t > 1)
        {
            if (transform.position.y <= HeadNote.transform.position.y || transform.position.y>= Column.HitBar.position.y)
            {
                Destroy(gameObject);
            }
        }
        else if (RhythmConductor.Instance.lastBeat >= InstantiationTimestamp / RhythmConductor.Instance.secondsPerNote)
        {
            if (HeadNote.BeingHit && transform.position.y <= Column.HitBar.transform.position.y)
            {
                var target = new Vector2(transform.position.x, Column.HitBar.position.y);
            }
            else
            {
                transform.position = Vector2.Lerp(Column.spawnPosition, Column.despawnPosition, (float)t);
            }
        }
    }
}
