using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Melanchall.DryWetMidi.Interaction;
using System;

public class LongLane : MonoBehaviour
{
    [SerializeField]
    private Melanchall.DryWetMidi.MusicTheory.NoteName musicalNote; //Lanes are asinged to each note of the piano roll so, for example, G notes are a lane, A# notes are another lane
    [SerializeField]
    private KeyCode inputButton;

    public GameObject NotePrefab;
    public GameObject LongNotePrefab;
    List<NoteScript> notes;
    private List<double> noteTimestamps;
    private Dictionary<double, float> tsLengthMap;//double = timeStamp float = noteLength

    int spawnIndex = 0;
    int inputIndex = 0;

    void Awake()
    {
        notes = new List<NoteScript>();
        noteTimestamps = new List<double>();
        tsLengthMap = new Dictionary<double, float>();
    }
    public void SetTimestamps(Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == musicalNote)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                noteTimestamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                //Debug.Log("index " + (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f + "length " + note.Length);
                if (note.Length > 64)
                {
                    tsLengthMap.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f, note.Length/64f);
                }
                else
                {
                    tsLengthMap.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f, 1f);
                }
            }
        }
        //Debug.Log(noteTimestamps.Count);
    }
    void Update()
    {
        //Debug.Log($"spawnIndex: {spawnIndex}");
        //Debug.Log($"inputIndex: {inputIndex}");
        if (spawnIndex < noteTimestamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= noteTimestamps[spawnIndex] - SongManager.Instance.NoteTime)
            {
                float uwu;
                tsLengthMap.TryGetValue(noteTimestamps[spawnIndex], out uwu);
                GameObject note;
                if (uwu > 2)
                {
                    note = Instantiate(LongNotePrefab, transform);
                    var endNotePosition = note.GetComponent<LongNoteScript>().endNote.transform.position;
                    note.GetComponent<LongNoteScript>().endNote.transform.localPosition = new Vector2(0, transform.position.y - uwu);
                    note.GetComponent<LongNoteScript>().noteLength = uwu;
                }
                else
                {
                    note = Instantiate(NotePrefab, transform);
                    //note.GetComponent<NoteScript>().noteLength = uwu;
                }
                var noteScript = note.GetComponent<NoteScript>();
                note.GetComponent<SpriteRenderer>().enabled = true;
                noteScript.assignedTime = (float)noteTimestamps[spawnIndex];
                notes.Add(noteScript);
                spawnIndex++;
            }
        }
        if (inputIndex < noteTimestamps.Count)
        {
            double timeStamp = noteTimestamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelay / 1000.0);

            if (Input.GetKeyDown(inputButton))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    ScoreManager.Instance.NoteHit();

                    notes[inputIndex].NoteHit();
                    inputIndex++;
                }
                else
                {
                    ScoreManager.Instance.NoteMissed();
                    //Debug.Log($"Late Hit {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                ScoreManager.Instance.NoteMissed();
                //print($"Missed {inputIndex} note lane {gameObject.name}");
                inputIndex++;
            }
        }
    }
}
