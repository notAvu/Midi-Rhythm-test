using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteScript : NoteScript
{
    public GameObject HeadNoteGO { get; set; }
    public GameObject TailNoteGO { get; set; }
    public NoteScript HeadNote { get; private set; }
    public NoteScript TailNote { get; private set; }
    //HeadNote and TailNote may be good as a different class that also manages other properties that NoteScript doesnt have to work with (Like the line between them)
    public List<GameObject> NestedNotes { get; private set; }
    public double Duration
    {
        get => Duration;
        set
        {
            Duration = value;
            if(TailNote != null)
            {
                TailNote.instantiationTimestamp = EndTime;
            }
        }
    }
    public double StartTime
    {
        get => instantiationTimestamp;
        set
        {
            if (HeadNote != null)
            {
                HeadNote.instantiationTimestamp = value;
            }
            if (TailNote != null)
            {
                TailNote.instantiationTimestamp = EndTime;
            }
            instantiationTimestamp = value;
        }
    }
    public double EndTime
    {
        get => StartTime + Duration;
        set => Duration = value - StartTime;
    }
    //time between ticks 
    private double tickSpacing;
    //number of ticks per beat 
    private double tickRate;
    public GameObject endNote;
    private void Awake()
    {
        TailNote = TailNoteGO.GetComponent<NoteScript>();
        HeadNote = HeadNoteGO.GetComponent<NoteScript>();
        //endNote.transform.position = new Vector2(transform.position.x, transform.position.y + noteLength);
        //Debug.Log(noteLength);
        //bar.transform.localScale = new Vector2(transform.localScale.x,transform.localScale.y+noteLength);
    }
    private void Update()
    {
        //endNote.transform.position = new Vector2(transform.position.x, transform.position.y + noteLength);

        timeSienceInstantiated = SongManager.GetAudioSourceTime() - instantiationTimestamp;
        float t = (float)(timeSienceInstantiated / (SongManager.Instance.NoteTime * 2));
        if (t > 1)
        {
            //Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, SongManager.Instance.NoteSpawnPointY, 0), new Vector3(transform.position.x, SongManager.Instance.NoteDespawnY, 0), t);
        }
        SetLineRendererPoints();
    }
    private void SetLineRendererPoints()
    {
        var bar = gameObject.GetComponent<LineRenderer>();  
        bar.SetPosition(0, HeadNoteGO.transform.position);
        bar.SetPosition(1, TailNoteGO.transform.position);
    }
    private void CreateNestedElements()
    {
        //In Osu! Mania nested notes have HoldNoteTicks, normal notes nested inside long notes but with no HitWindows(I'm guessing always "perfect")
        //Other rhythm games have similar system, where long notes have several ticks between the head and tail note, so I guess Osu Mania is a good reference point for this game 
        
        //Still have to figure out where to set
        AddNestedNote(HeadNote);
        AddNestedNote(TailNote);
        tickSpacing = SongManager.Instance.crochet / tickRate;
        for(double i = StartTime+ tickSpacing; i <= EndTime-tickSpacing; i+=tickSpacing)
        {
            //Create a new note class for ticks? 
            //AddNestedNote();
        }
    }
    private void AddNestedNote(NoteScript note)
    {
        
    }
}
