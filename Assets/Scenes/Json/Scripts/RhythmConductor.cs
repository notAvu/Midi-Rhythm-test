using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RhythmConductor : MonoBehaviour
{
    private static RhythmConductor instance;
    public static RhythmConductor Instance
    {
        get
        {
            //if(instance == null)
            //{
            //    instance = new GameObject().AddComponent<RhythmConductor>();
            //    instance.name = "RhythmConductor";
            //}
            return instance;
        }
    }
    public TextAsset jsonFile;
    private AudioSource audioSource;
    public SongDataContainer songFiles;
    [SerializeField]
    private GameObject beatBar;

    private int songBpm;
    private int notesPerBeat;
    public double secondsPerNote;

    public float offset;
    [HideInInspector]
    public int columnCount;

    public double songPosition;
    public double songPositionSeconds;
    public double lastBeat;
    private double dpsTime;

    private List<SpawnColumn> lanes;

    private void Awake()
    {
        songFiles = GameObject.FindGameObjectWithTag("SongLoader").GetComponent<FancySongSelect>().selectedSong.songDataContainer;
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        this.jsonFile = songFiles.beatmapJson;
        ReadBeatmapInfo();
    }
    private void FindLanes()
    {
        lanes = new List<SpawnColumn>();
        var lanesGo= GameObject.FindGameObjectsWithTag("Lane").ToList();
        lanesGo.ForEach(a => lanes.Add(a.GetComponent<SpawnColumn>()));
    }
    private void Start()
    {
        lastBeat = 0;
        audioSource = GetComponent<AudioSource>();
        this.audioSource.clip = songFiles.audioClip;
        dpsTime = AudioSettings.dspTime;
        secondsPerNote = 60f / (songBpm * notesPerBeat);
        FindLanes();
        InstantiateWholeMap();
        //songPositionSeconds = (float)(AudioSettings.dspTime - dpsTime) + offset;
        Invoke(nameof(PlaySong), offset);
    }
    private void PlaySong() {
        audioSource.Play();
    }
    private void Update()
    {
        songPositionSeconds = (float)((AudioSettings.dspTime - dpsTime) );
        songPosition = songPositionSeconds / secondsPerNote;
        if (lastBeat > 0 && (int)lastBeat != BeatBar.LastIndex && lastBeat % 8 == 0)
        {
            BeatBar.LastIndex = (int)lastBeat;
            //SpawnBar();
        }
        if (songPosition > lastBeat + secondsPerNote)
        {
            lastBeat += secondsPerNote;
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
        columnCount = beatmapInfo.maxBlock;
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
    //private void SpawnBar()//This is actually just trash xd
    //{
    //    var uwu = Instantiate(beatBar);
    //    uwu.GetComponent<BeatBar>().CurrentIndex = (int)lastBeat;
    //    uwu.GetComponent<BeatBar>().InstantiationTimestamp = lastBeat * secondsPerNote;
    //}
    public double GetAudioSourceTime()
    {
        return (double) audioSource.timeSamples / audioSource.clip.frequency;
    }
}