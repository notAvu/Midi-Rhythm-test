using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Quick skecth of the longNoteScript 
public class LongNote : MonoBehaviour
{
    public NoteObject noteData;
    GameObject headNote;
    public SpawnColumn Column;
    /// <summary>
    /// The timestamp at which this note starts moving in the screen
    /// </summary>
    public float InstantiationTimestamp;
    public float StartTime;//TODO: APPLY THE PATTERN OsuMania USES
    public GameObject tailNote;
    public float EndTime;
    /// <summary>
    /// The list of ticks that forms the long note
    /// </summary>
    public List<GameObject> nestedNotes;
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject TickNotePrefab;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        headNote = gameObject;//change for better structure
    }
    private void Update()
    {
        //TimeSienceInstantiation = conductor.songPositionSeconds - InstantiationTimestamp;
        //var t = TimeSienceInstantiation;
        //Debug.Log(t);
        //if (t > 1)
        //{
        //    //Destroy(gameObject);
        //}
        //else if (conductor.lastBeat >= InstantiationTimestamp / conductor.secondsPerNote)
        //{
        //    transform.position = Vector2.Lerp(column.spawnPosition, column.despawnPosition, t);
        //}
    }
    private void LateUpdate()
    {
        SetLinerendererPoints();
    }
    /// <summary>
    /// Instantiates a TickNotePrefab(<seealso cref="TickNoteScript"/>) and sets is as part of the same long note
    /// </summary>
    public void InstantiateNestedNotes()
    {
        nestedNotes = new List<GameObject>();
        foreach (var nested in noteData.nestedNotes)
        {
            GameObject note = Instantiate(TickNotePrefab);
            //note.transform.SetParent(gameObject.transform);
            note.GetComponent<TickNoteScript>().noteData = nested;
            note.GetComponent<TickNoteScript>().Column = this.Column;
            nestedNotes.Add(note);
        }
        tailNote = nestedNotes[nestedNotes.Count - 1];
    }
    /// <summary>
    /// Sets the linerenderer points to connect the head note with every other object in the same long note  
    /// </summary>
    private void SetLinerendererPoints()
    {
        lineRenderer.positionCount = nestedNotes.Count;
        lineRenderer.SetPosition(lineRenderer.positionCount-1, headNote.transform.position);
        for (int i = (nestedNotes.Count-1); i >= 0; i--)
        {
            var position = nestedNotes[i].GetComponent<Transform>().position;
            lineRenderer.SetPosition(i, position);
        }
    }
}
