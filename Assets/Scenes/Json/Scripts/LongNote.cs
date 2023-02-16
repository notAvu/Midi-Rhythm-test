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
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject TickNotePrefab;
    private void Awake()
    {
        this.gameObject.transform.position = new Vector3(0, 6, 0);
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        headNote = gameObject;//change for better structure
    }
    private void LateUpdate()
    {
        SetLinerendererPoints();
    }
    public void InstantiateNestedNotes()
    {
        nestedNotes = new List<GameObject>();
        foreach (var nested in noteData.nestedNotes)
        {
            GameObject note = Instantiate(TickNotePrefab);
            //note.transform.SetParent(gameObject.transform);
            note.GetComponent<TickNoteScript>().noteData = nested;
            nestedNotes.Add(note);
        }
        tailNote = nestedNotes[nestedNotes.Count - 1];
    }
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
