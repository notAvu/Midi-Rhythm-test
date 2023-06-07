using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class SpawnColumn : MonoBehaviour
{
    #region constants
    public const float COLUMN_WIDTH = 0;
    #endregion
    #region components
    [SerializeField]
    private AudioSource hitAudioSource;
    [SerializeField]
    private AudioSource missAudioSource;
    #endregion
    #region gameObject attributes
    public int ColumnIndex;
    [SerializeField]
    public Vector2 spawnPosition;
    [SerializeField]
    public Vector2 despawnPosition;
    [SerializeField]
    private KeyCode input;//TODO: Establecer el sistema de input 
    #endregion
    #region notes
    [SerializeField]
    [Header("Single hit note prefab")]
    private GameObject singleNotePrefab;
    [SerializeField]
    [Header("Long note prefab")]
    private GameObject longNotePrefab;
    private List<GameObject> notes = new List<GameObject>();
    private int inputIndex; //the index of thenext note to be hit in this lane 
    #endregion
    #region TODO: rework conductor system as a singleton or similar
    RhythmConductor conductor;
    #endregion
    [HideInInspector]
    public Transform HitBar;
    #region unity events
    private void Awake()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
        HitBar = GameObject.Find("HitBar").transform;
    }
    private void FixedUpdate()
    {
        Debug.Log($"Input Index: {inputIndex} \n Lane {ColumnIndex}");
    }
    #endregion
    #region Input management
    private void OnInput()
    {
        var currentNoteTs = notes[inputIndex].GetComponent<NoteScript>().assignedTime;
        var songTime = conductor.songPositionSeconds;
        var hitWindowDiff = conductor.secondsPerNote*.3f;
        if (true/*Input.GetKeyDown(input)*/)
        {
            if (Mathf.Abs(songTime-currentNoteTs)<hitWindowDiff)
            {
                ScoreManager.Instance.NoteHit();
                hitAudioSource.Play();
            }
            else
            {
                ScoreManager.Instance.NoteMissed();
                missAudioSource.Play();
            }
        }
    }
    #endregion
    #region note instantiation
    // This may be better done as a coroutine that loads the level in a loading screen before playing the actual song
    /// <summary>
    /// Generates the notes that are going to be played in this column
    /// </summary>
    /// <param name="noteList"></param>
    public void InstantiateNotes(List<NoteObject> noteList)
    {
        foreach (var note in noteList)
        {
            if (note.Type == NoteTypes.SingleHit)
            {
                AddSingleNote(note);
            }
            else if (note.Type == NoteTypes.LongNote)
            {
                AddLongNote(note);
            }
        }
    }
    /// <summary>
    /// Instantiates and adds a long note to the list of notes that will appear in this column
    /// </summary>
    /// <param name="note">A<seealso cref="NoteObject"/> object containing the data extracted from the json file abt this note</param>
    private void AddLongNote(NoteObject note)
    {
        var longNoteHead = Instantiate(longNotePrefab);
        longNoteHead.transform.position = spawnPosition;
        LongNote longNoteScript = longNoteHead.GetComponent<LongNote>();
        longNoteScript.NoteData = note;
        longNoteScript.Column = this;
        longNoteScript.InstantiateNestedNotes();
        longNoteScript.StartTime = longNoteScript.NoteData.NoteIndex * conductor.secondsPerNote + conductor.offset;
        longNoteScript.InstantiationTimestamp = longNoteScript.StartTime - (conductor.secondsPerNote * 8);
        longNoteScript.NestedNotes.ForEach(nested =>
        {
            nested.GetComponent<TickNoteScript>().NoteTimestamp = nested.GetComponent<TickNoteScript>().noteData.NoteIndex * conductor.secondsPerNote + conductor.offset;
            nested.GetComponent<TickNoteScript>().InstantiationTimestamp = nested.GetComponent<TickNoteScript>().NoteTimestamp - (conductor.secondsPerNote * 8);
            nested.GetComponent<TickNoteScript>().Column = this;
            notes.Add(nested);
        });
        longNoteScript.EndTime = longNoteScript.TailNote.GetComponent<TickNoteScript>().NoteTimestamp;
        notes.Add(longNoteHead);
    }
    /// <summary>
    /// Instantiates and adds a single hit note to the list of notes that will appear in this column
    /// </summary>
    /// <param name="note">A<seealso cref="NoteObject"/> object containing the data extracted from the json file abt this note</param>
    private void AddSingleNote(NoteObject note)
    {
        var newNote = Instantiate(singleNotePrefab);
        newNote.transform.position = spawnPosition;
        SingleHitNote noteScript = newNote.GetComponent<SingleHitNote>();
        noteScript.noteData = note;
        noteScript.conductor = conductor;
        noteScript.column = this;
        noteScript.NoteTimestamp = noteScript.noteData.NoteIndex * conductor.secondsPerNote + conductor.offset;
        noteScript.InstantiationTimestamp = noteScript.NoteTimestamp - (conductor.secondsPerNote * 8); //TODO: ajustar, probablemente este sea el problema que hace que el timing vaya regu
        notes.Add(newNote);
    }
    #endregion
}
