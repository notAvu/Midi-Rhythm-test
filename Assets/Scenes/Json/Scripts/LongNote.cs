using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Quick skecth of the longNoteScript 
public class LongNote : MonoBehaviour
{
    public NoteObject noteData;
    GameObject headNote;
    RhythmConductor conductor;
    public float StartTime;//TODO: APPLY THE PATTERN OsuMania USES
    public GameObject tailNote;
    public float EndTime;
    public List<GameObject> nestedNotes;
    LineRenderer lineRenderer;
    [SerializeField]
    private GameObject TickNotePrefab;
    private void Awake()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
        nestedNotes = new List<GameObject>();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        headNote = gameObject;//change for better structure
        InstantiateNestedNotes();
    }
    private void Start()
    {
    }
    private void Update()
    {
        SetLinerendererPoints();
    }
    private void InstantiateNestedNotes()
    {
        var lastNote = noteData.nestedNotes[noteData.nestedNotes.Count - 1];
        noteData.nestedNotes.RemoveAt(noteData.nestedNotes.Count - 1);//Do I need this?
        foreach (var nested in noteData.nestedNotes)
        {
            GameObject note = Instantiate(TickNotePrefab);
            note.GetComponent<TickNoteScript>().noteData = nested;
            //TODO set timeStamps timestamp
            nestedNotes.Add(note);
        }
        SetLinerendererPoints();
    }
    private void SetLinerendererPoints() 
    {
        lineRenderer.SetPosition(0, headNote.transform.position);
        for (int i = 0; i < nestedNotes.Count; i++)
        {
            var position = nestedNotes[i].GetComponent<Transform>().position;
            lineRenderer.SetPosition(i+1, position);
        }
    }
}
