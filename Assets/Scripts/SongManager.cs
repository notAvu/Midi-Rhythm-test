using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource songAudio;
    
    private float songDelay; //time between the midi file loading and the song starting
    public int inputDelay; //in milliseconds
    [SerializeField]
    public double marginOfError; //seconds 

    [SerializeField]
    private string fileLocation; //midi file location in streamingassets 
    public static MidiFile midiFile; //loaded midi file

    [SerializeField]
    private Lane[] lanes;

    [SerializeField]
    private LongLane[] longLanes;
    [SerializeField]
    public float NoteSpawnPointY;
    [SerializeField]
    public float NoteTime;
    [SerializeField]
    private float noteTapY;
    [SerializeField]
    public float NoteDespawnY { get { return noteTapY - (NoteSpawnPointY-noteTapY) ; } }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadMidiFile();
    }
    private void ReadMidiFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        var notes = midiFile.GetNotes();
        var noteArray = new Note[notes.Count];
        notes.CopyTo(noteArray,0);

        foreach(var lane in lanes)
        {
            lane.SetTimestamps(noteArray);
        }
        foreach (var longLane in longLanes)
        {
            longLane.SetTimestamps(noteArray);
        }
        //manipulation here
        Invoke(nameof(StartSong), songDelay);

    }

    private void StartSong()
    {
        songAudio.Play();
    }
    public static double GetAudioSourceTime()
    {
        return (double)Instance.songAudio.timeSamples / Instance.songAudio.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
