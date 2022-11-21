using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Melanchall.DryWetMidi.Interaction;
using System;

public class Lane : MonoBehaviour
{
    [SerializeField]
    private Melanchall.DryWetMidi.MusicTheory.NoteName musicalNote; //Lanes are asinged to each note of the piano roll so, for example, G notes are a lane, A# notes are another lane
    [SerializeField]
    private KeyCode inputButton;

    public GameObject NotePrefab;
    List<NoteScript> notes;
    private List<double> noteTimestamps;

    int spawnIndex = 0;
    int inputIndex = 0;

    void Start()
    {
        notes = new List<NoteScript>();
        noteTimestamps = new List<double>();
    }
    public void SetTimestamps(Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == musicalNote)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                noteTimestamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    void Update()
    {
        if(spawnIndex < noteTimestamps.Count)
        {
            var note = Instantiate(NotePrefab, transform);
            notes.Add(note.GetComponent<NoteScript>());
            note.GetComponent<NoteScript>().assignedTime = (float)noteTimestamps[spawnIndex];
            spawnIndex++;
        }
        if(inputIndex < noteTimestamps.Count)
        {
            double timeStamp = noteTimestamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelay / 1000.0);

            if (Input.GetKeyDown(inputButton))
            {
                if(Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    ScoreManager.Instance.NoteHit();
                    inputIndex++;
                }
                else
                {
                    //miss note
                    Debug.Log($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                ScoreManager.Instance.NoteMissed();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }
}
