using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Quick skecth of the longNoteScript 
/// <summary>
/// <strong>
/// TODO: make headNote fall (Implement instantiation ts and that kind of stuff)
/// </strong>
/// </summary>
public class LongNote : MonoBehaviour
{
    #region Attributes
    private RhythmConductor conductor;
    public NoteObject NoteData;
    public SpawnColumn Column;
    public GameObject TailNote;
    public List<GameObject> NestedNotes;
    public bool BeingHit;
    /// <summary>
    /// The timestamp at which this note starts moving in the screen
    /// </summary>
    public float InstantiationTimestamp;
    public float TimeSinceInstantiation;
    public float StartTime;//TODO: APPLY THE PATTERN OsuMania USES
    public float EndTime;
    /// <summary>
    /// The list of ticks that forms the long note
    /// </summary>
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject TickNotePrefab;
    #endregion
    #region Unity Events
    private void Awake()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    private void Start()
    {
        BeingHit = true;// Just for testing, must change when input is correctly implemented
        foreach (var note in NestedNotes)
        {
            note.GetComponent<TickNoteScript>().HeadNote = this;
        }
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        TimeSinceInstantiation = conductor.songPositionSeconds - InstantiationTimestamp;
        var t = TimeSinceInstantiation;
        Debug.Log(t);
        if (t > 1)
        {
            if (TailNote.gameObject == null)
            {
                Destroy(gameObject);
            }
        }
        else if (conductor.lastBeat >= InstantiationTimestamp / conductor.secondsPerNote)
        {
            if (BeingHit && transform.position.y <= Column.HitBar.transform.position.y)
            {
                transform.position = new Vector2(transform.position.x, Column.HitBar.position.y);
            }
            else
            {
                transform.position = Vector2.Lerp(Column.spawnPosition, Column.despawnPosition, t);
            }
        }
    }
    private void LateUpdate()
    {
        SetLinerendererPoints();
    }
    #endregion
    #region Note Instantiation
    /// <summary>
    /// Instantiates a <seealso cref="TickNoteScript"/> GO and sets is as part of the same long note
    /// </summary>
    public void InstantiateNestedNotes()
    {
        NestedNotes = new List<GameObject>();
        foreach (var nested in NoteData.nestedNotes)
        {
            var tailnoteIndex = 0f;
            GameObject note = Instantiate(TickNotePrefab);
            //note.transform.SetParent(gameObject.transform);
            note.GetComponent<TickNoteScript>().noteData = nested;
            note.GetComponent<TickNoteScript>().Column = this.Column;
            note.GetComponent<Transform>().position = Column.transform.position;
            NestedNotes.Add(note);
            if (nested.NoteIndex > tailnoteIndex)
            {
                tailnoteIndex = nested.NoteIndex;
                TailNote = note;
            }
        }
    }
    #endregion
    #region Visuals related methods
    /// <summary>
    /// Sets the linerenderer points to connect the head note with every other object in the same long note  
    /// </summary>
    private void SetLinerendererPoints()
    {
        lineRenderer.positionCount = NoteData.nestedNotes.Count + 1;
        lineRenderer.SetPosition(0, transform.position);
        for (int i = (NestedNotes.Count); i >= 1; i--)
        {
            if (NestedNotes[i - 1] != null)
            {
                var position = NestedNotes[i - 1].GetComponent<Transform>().position;
                lineRenderer.SetPosition(i, position);
            }
            else { lineRenderer.positionCount = NestedNotes.Count+1; }
        }
    }
    #endregion
}
