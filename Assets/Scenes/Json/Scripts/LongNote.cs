using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Quick skecth of the longNoteScript 
public class LongNote : MonoBehaviour
{
    public NoteObject noteData;
    GameObject headNote;
    RhythmConductor conductor;
    public float InstantiationTimestamp;
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
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        headNote = gameObject;//change for better structure
    }
    private void Update()
    {
        SetLinerendererPoints();
    }
    public void InstantiateNestedNotes()
    {
        nestedNotes = new List<GameObject>();
        foreach (var nested in noteData.nestedNotes)
        {
            GameObject note = Instantiate(TickNotePrefab);
            note.transform.SetParent(gameObject.transform);
            note.GetComponent<TickNoteScript>().noteData = nested;
            //TODO set timeStamps timestamp
            nestedNotes.Add(note);
        }
        tailNote = nestedNotes[nestedNotes.Count - 1];
    }
    private void SetLinerendererPoints()
    {
        lineRenderer.SetPosition(0, headNote.transform.position);
        for (int i = 0; i < nestedNotes.Count; i++)
        {
            var position = nestedNotes[i].GetComponent<Transform>().position;
            lineRenderer.SetPosition(i + 1, position);
        }
    }
}
