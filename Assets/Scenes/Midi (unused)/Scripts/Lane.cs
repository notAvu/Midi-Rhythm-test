using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Melanchall.DryWetMidi.Interaction;
using System;
[Obsolete("Midi System is deprecated, this project is currently using the JSON system for mapping")]
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

    void Awake()
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
                var note = Instantiate(NotePrefab, transform);
                note.GetComponent<SpriteRenderer>().enabled = true;
                note.GetComponent<NoteScript>().assignedTime = (float)noteTimestamps[spawnIndex];
                note.GetComponent<NoteScript>().laneId = musicalNote.ToString();
                notes.Add(note.GetComponent<NoteScript>());
                spawnIndex++;
            }
        }
        if (inputIndex < noteTimestamps.Count)
        {
            double timeStamp = noteTimestamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelay / 1000.0);
            double hitDiff = Math.Abs(audioTime - timeStamp);
            if (Input.GetKeyDown(inputButton))
                //maybe check keydown only for 1hit notes and getkey for long notes 
                // UPDATE : There may not be necessary to check every note as long notes have nested notes and you should only check nested notes of active long notes 
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
