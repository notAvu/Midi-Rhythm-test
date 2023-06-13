using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    #endregion
    #region notes
    [SerializeField]
    [Header("Single hit note prefab")]
    private GameObject singleNotePrefab;
    [SerializeField]
    [Header("Long note prefab")]
    private GameObject longNotePrefab;
    private List<GameObject> notes = new List<GameObject>();
    public int InputIndex { get; set; } //the index of thenext note to be hit in this lane 
    #endregion
    [HideInInspector]
    public Transform HitBar; //Hitbar is just a visual help to identify when the player should hit notes
    #region input 
    private RhythmInput inputActions;
    private InputAction inputAction;
    #endregion
    #region unity events
    private void Awake()
    {
        inputActions = new RhythmInput();
        HitBar = GameObject.Find("HitBar").transform;
    }
    private void OnEnable()
    {
        inputAction = ColumnIndex switch
        {
            0 => inputActions.SongInput.Lane1Input,
            1 => inputActions.SongInput.Lane2Input,
            2 => inputActions.SongInput.Lane3Input,
            3 => inputActions.SongInput.Lane4Input,
            _ => null
        };
        inputAction.performed += OnButtonPress;
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }
    private void FixedUpdate()
    {
        //Debug.Log($"Input Index: {InputIndex} \n Lane {ColumnIndex}  notes:{notes.Count}");
    }
    #endregion
    #region Input management
    /// <summary>
    /// Checks the note that should currently be hit/missed and triggers 
    /// the coresponding note's <seealso cref="IHitObject"/> method
    /// </summary>
    private void OnButtonPress(InputAction.CallbackContext context)
    {
        var currentNoteTs = notes[InputIndex].GetComponent<HitNote>().NoteTimestamp;
        var note = notes[InputIndex];
        var songTime = RhythmConductor.Instance.songPositionSeconds;
        var hitWindowDiff = .15f;
        var hitDiff = Math.Abs(songTime - currentNoteTs);
        if (hitDiff < hitWindowDiff)
        {
            if (note.GetComponent<IHitObject>().GetType().Equals(typeof(SingleHitNote)))
            {
                note.GetComponent<IHitObject>().Hit();
                hitAudioSource.Play();
            }
        }
        else if (hitDiff > hitWindowDiff && hitDiff < 0.25f)
        {
            if (note.GetComponent<IHitObject>().GetType().Equals(typeof(SingleHitNote)))
            {
                note.GetComponent<IHitObject>().Miss();
            }
        }
    }
    private void OnButtonRelease(InputAction.CallbackContext context)
    {

    }
    private void LongNoteHit()
    {

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
        longNoteScript.StartTime = longNoteScript.NoteData.NoteIndex * RhythmConductor.Instance.secondsPerNote + RhythmConductor.Instance.offset;
        longNoteScript.InstantiationTimestamp = longNoteScript.StartTime - (RhythmConductor.Instance.secondsPerNote * 8);
        longNoteScript.NestedNotes.ForEach(nested =>
        {
            nested.GetComponent<TickNoteScript>().NoteTimestamp = nested.GetComponent<TickNoteScript>().noteData.NoteIndex * RhythmConductor.Instance.secondsPerNote + RhythmConductor.Instance.offset;
            nested.GetComponent<TickNoteScript>().InstantiationTimestamp = nested.GetComponent<TickNoteScript>().NoteTimestamp - (RhythmConductor.Instance.secondsPerNote * 8);
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
        //noteScript.RhythmConductor.Instance = RhythmConductor.Instance;
        noteScript.column = this;
        noteScript.NoteTimestamp = noteScript.noteData.NoteIndex * RhythmConductor.Instance.secondsPerNote + RhythmConductor.Instance.offset;
        noteScript.InstantiationTimestamp = noteScript.NoteTimestamp - (RhythmConductor.Instance.secondsPerNote * 8); //TODO: ajustar, probablemente este sea el problema que hace que el timing vaya regu
        notes.Add(newNote);
    }
    #endregion
}
