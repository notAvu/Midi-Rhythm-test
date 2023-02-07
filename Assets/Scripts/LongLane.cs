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
    List<float> noteLengthList;

    int spawnIndex = 0;
    int inputIndex = 0;

    void Awake()
    {
        notes = new List<NoteScript>();
        noteTimestamps = new List<double>();
        //tsLengthMap = new Dictionary<double, float>();
        noteLengthList = new List<float>();
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
                    noteLengthList.Add(note.Length / 64f);
                }
                else
                {
                    noteLengthList.Add(1f);
                }
            }
        }
        //Debug.Log(noteTimestamps.Count);
    }
    void Update()
    {
        if (spawnIndex < noteTimestamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= noteTimestamps[spawnIndex] - SongManager.Instance.NoteTime)
            {
                float uwu = noteLengthList[spawnIndex];
                GameObject note;
                NoteScript noteScript;
                if (uwu <= 1)
                {
                    note = Instantiate(NotePrefab, transform);
                    //note.GetComponent<NoteScript>().noteLength = uwu;
                    noteScript = note.GetComponent<NoteScript>();
                    noteScript.assignedTime = (float)noteTimestamps[spawnIndex];
                }
                else
                {
                    note = Instantiate(LongNotePrefab, transform);
                    //note.GetComponent<LongNoteScript>().noteLength = uwu;
                    noteScript = note.GetComponent<LongNoteScript>();
                    noteScript.assignedTime = (float)noteTimestamps[spawnIndex];
                }
                note.GetComponent<SpriteRenderer>().enabled = true;
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
