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
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        TimeSinceInstantiation = conductor.songPositionSeconds - InstantiationTimestamp;
        var t = TimeSinceInstantiation;
        Debug.Log(t);
        if (t > 1)
        {
            //Destroy(gameObject);
        }
        else if (conductor.lastBeat >= InstantiationTimestamp / conductor.secondsPerNote)
        {
            transform.position = Vector2.Lerp(Column.spawnPosition, Column.despawnPosition, t);
        }
    }
    private void LateUpdate()
    {
        SetLinerendererPoints();
    }
    #endregion
    #region Note Instantiation
    /// <summary>
    /// Instantiates a TickNotePrefab(<seealso cref="TickNoteScript"/>) and sets is as part of the same long note
    /// </summary>
    public void InstantiateNestedNotes()
    {
        NestedNotes = new List<GameObject>();
        foreach (var nested in NoteData.nestedNotes)
        {
            GameObject note = Instantiate(TickNotePrefab);
            //note.transform.SetParent(gameObject.transform);
            note.GetComponent<TickNoteScript>().noteData = nested;
            note.GetComponent<TickNoteScript>().Column = this.Column;
            NestedNotes.Add(note);
        }
        TailNote = NestedNotes[NestedNotes.Count - 1];
    }
    #endregion
    #region Visuals related methods
    /// <summary>
    /// Sets the linerenderer points to connect the head note with every other object in the same long note  
    /// </summary>
    private void SetLinerendererPoints()
    {
        lineRenderer.positionCount = NestedNotes.Count;
        lineRenderer.SetPosition(lineRenderer.positionCount-1, gameObject.transform.position);
        for (int i = (NestedNotes.Count-1); i >= 0; i--)
        {
            var position = NestedNotes[i].GetComponent<Transform>().position;
            lineRenderer.SetPosition(i, position);
        }
    }
    #endregion
}
