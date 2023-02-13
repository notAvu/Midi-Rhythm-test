using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RhythmConductor : MonoBehaviour
{
    public TextAsset jsonFile;
    private AudioSource songAudio;
    [SerializeField]
    private GameObject beatBar;

    private int songBpm;
    private int notesPerBeat;
    private float secondsPerNote;//Should this be calculated for actual beats or chart beats (Subdivided)

    private double offset;
    private int columns;

    public float songPosition;
    public float songPositionSeconds;
    float lastBeat;
    private float dpsTime;

    private void Awake()
    {
        ReadBeatmapInfo();
    }
    private void Start()
    {
        lastBeat = 0;
        songAudio = GetComponent<AudioSource>();
        dpsTime = (float)AudioSettings.dspTime;
        secondsPerNote = 60f / (songBpm * notesPerBeat);
        //secondsPerBeat = 60f / songBpm ;
        songAudio.Play();
    }
    private void Update()
    {
        songPositionSeconds = (float)((AudioSettings.dspTime - dpsTime) - offset);
        songPosition = songPositionSeconds / secondsPerNote;
        //Debug.Log($"songPosition: {songPosition}");
        if ((int)lastBeat % 4 == 0 && lastBeat > 0 && (int)lastBeat != SingleHitNote.lastIndex)
        {
            SingleHitNote.lastIndex = (int)lastBeat;
            SpawnBar();
        }
        if (songPosition > lastBeat + secondsPerNote)
        {
            lastBeat += secondsPerNote;
            //Debug.Log($"LastBeat: {(int)lastBeat}");
        }
    }
    private void SpawnBar()
    {
        var uwu = Instantiate(beatBar);
        uwu.GetComponent<SingleHitNote>().index = (int)lastBeat;
        uwu.GetComponent<SingleHitNote>().InstantiationTimestamp = lastBeat * secondsPerNote;
    }
    private void ReadBeatmapInfo()
    {
        JsonBeatmapParser jsonParser = new JsonBeatmapParser();
        var beatmapInfo = jsonParser.ParseBeatmap(jsonFile);
        songBpm = beatmapInfo.BPM;
        notesPerBeat = beatmapInfo.notes[0].LPB;
        offset = beatmapInfo.offset / 1000;
        columns = beatmapInfo.maxBlock;
    }
}
