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
    private AudioSource songAudio;
    
    private float songDelay; //time between the midi file loading and the song starting
    private int inputDelay; //in milliseconds

    public double marginOfError; //seconds 

    private string fileLocation; //midi file location in streamingassets 
    public static MidiFile midiFile; //loaded midi file

    public float NoteSpawnPointY { get; set; }
    public float NoteTime { get; set; }
    private float noteTapY;
    public float NoteDespawnY { get { return noteTapY - (NoteSpawnPointY-noteTapY) ; } }

    // Start is called before the first frame update
    void Start()
    {
    }
    private void ReadMidiFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        var notes = midiFile.GetNotes();
        var noteArray = new Note[notes.Count];
        notes.CopyTo(noteArray,0);

        //manipulation here

        Invoke(nameof(StartSong), songDelay);

    }

    private void StartSong()
    {
        songAudio.Play();

    }
    public static double GetAudioSourceTime()
    {
        return Instance.songAudio.timeSamples / Instance.songAudio.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
