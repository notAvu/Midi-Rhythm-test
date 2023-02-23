using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float secondsPerNote;

    public float offset;
    private int columns;

    public float songPosition;
    public float songPositionSeconds;
    public float lastBeat;
    private float dpsTime;

    private List<SpawnColumn> lanes;
    private void Awake()
    {
        ReadBeatmapInfo();
    }
    private void Start()
    {
        InstantiateWholeMap();
        lastBeat = 0;
        songAudio = GetComponent<AudioSource>();
        dpsTime = (float)AudioSettings.dspTime;
        secondsPerNote = 60f / (songBpm * notesPerBeat);
        songAudio.Play();
    }
    private void Update()
    {
        songPositionSeconds = (float)((AudioSettings.dspTime - dpsTime) - offset);
        songPosition = songPositionSeconds / secondsPerNote;
        //Debug.Log($"songPosition: {songPosition}");
        if (lastBeat > 0 && (int)lastBeat != BeatBar.LastIndex && lastBeat % 8 == 0)
        {
            BeatBar.LastIndex = (int)lastBeat;
            SpawnBar();
        }
        if (songPosition > lastBeat + secondsPerNote)
        {
            lastBeat += secondsPerNote;
            //Debug.Log($"LastBeat: {(int)lastBeat}");
        }
    }
    /// <summary>
    /// Reads the json file that contains the parameters of this song (bpm, offset, etc.) and notes 
    /// </summary>
    /// <returns>A list of <seealso cref="NoteObject"/>containing the data of every note in this beatmap</returns>
    private List<NoteObject> ReadBeatmapInfo()
    {
        List<NoteObject> notes = new List<NoteObject>();
        JsonBeatmapParser jsonParser = new JsonBeatmapParser();
        var beatmapInfo = jsonParser.ParseBeatmap(jsonFile);
        songBpm = beatmapInfo.BPM;
        notesPerBeat = beatmapInfo.notes[0].LPB;
        offset = (float)beatmapInfo.offset / 1000f;
        columns = beatmapInfo.maxBlock;
        beatmapInfo.notes.ToList().ForEach(item => { var noteObj = new NoteObject(item); notes.Add(noteObj);});
        return notes;
    }
    /// <summary>
    /// Instantiates columns and notes of this beat <br/>
    /// <strong>
    /// TODO: Instantiate columns dynamically
    /// </strong>
    /// </summary>
    private void InstantiateWholeMap()
    {
        ReadBeatmapInfo();
        List<NoteObject> beatmapNotes = ReadBeatmapInfo();
        foreach (var l in lanes)
        {
            var columnNotes= beatmapNotes.Where(n => n.Column ==l.ColumnIndex).ToList();
            l.InstantiateNotes(columnNotes);
        }
    }
    private void SpawnBar()//This is actually just trash xd
    {
        var uwu = Instantiate(beatBar);
        uwu.GetComponent<BeatBar>().CurrentIndex = (int)lastBeat;
        uwu.GetComponent<BeatBar>().InstantiationTimestamp = lastBeat * secondsPerNote;
    }
}