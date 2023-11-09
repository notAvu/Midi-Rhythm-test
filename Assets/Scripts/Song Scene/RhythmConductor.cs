using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RhythmConductor : MonoBehaviour
{
    #region singleton
    private static RhythmConductor instance;
    public static RhythmConductor Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion
    #region External data
    public TextAsset jsonFile;
    private AudioSource audioSource;
    public SongDataContainer songFiles;
    #endregion
    #region USELESS STUFF
    [SerializeField]
    private GameObject beatBar;
    #endregion
    #region song info
    private int songBpm;
    private int notesPerBeat;
    public double secondsPerNote;
    public float offset;
    [HideInInspector]
    public int columnCount;
    #endregion
    #region new rhythm sync system values
    private double currentSmoothDspTime;
    private double alphaValue;
    private double betaValue;
    private List<double> gameTimeValues;
    private List<double> dspTimeValues;
    #endregion
    #region song state 
    public double songPosition;
    public double songPositionSeconds;
    public double lastBeat;
    private double dpsTime;
    #endregion
    private List<SpawnColumn> lanes;
    #region Unity Events
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
        jsonFile = songFiles.beatmapJson;
        ReadBeatmapInfo();
    }

    private void Start()
    {
        lastBeat = 0;
        audioSource = GetComponent<AudioSource>();
        var timeLeft = audioSource.clip.length - audioSource.time;
        this.audioSource.clip = songFiles.audioClip;
        dpsTime = AudioSettings.dspTime;
        secondsPerNote = 60f / (songBpm * notesPerBeat);
        FindLanes();
        InstantiateWholeMap();
        //songPositionSeconds = (float)(AudioSettings.dspTime - dpsTime) + offset;
        Invoke(nameof(PlaySong), offset);
    }
    private void PlaySong()
    {
        audioSource.PlayScheduled(1f);
    }
    private void Update()
    {
        #region
        //if(gameTimeValues.Count()<10)
        #endregion

        songPositionSeconds = (float)(AudioSettings.dspTime - dpsTime);
        songPosition = songPositionSeconds / secondsPerNote;
        if (songPosition > lastBeat + secondsPerNote)
        {
            lastBeat += secondsPerNote;
        }
        //if (lastBeat > 0 && (int)lastBeat != BeatBar.LastIndex && lastBeat % 8 == 0)
        //{
        //    //BeatBar.LastIndex = (int)lastBeat;
        //    //SpawnBar();
        //}
    }
    #endregion
    #region initial instantiation methods
    private void FindLanes()
    {
        lanes = new List<SpawnColumn>();
        var lanesGo = GameObject.FindGameObjectsWithTag("Lane").ToList();
        lanesGo.ForEach(a => lanes.Add(a.GetComponent<SpawnColumn>()));
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
        beatmapInfo.notes.ToList().ForEach(item => { var noteObj = new NoteObject(item); notes.Add(noteObj); });
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
            var columnNotes = beatmapNotes.Where(n => n.Column == l.ColumnIndex).ToList();
            l.InstantiateNotes(columnNotes);
        }
    }
    #endregion
    #region timing stuff
    public double GetAudioSourceTime()
    {
        return (double)audioSource.timeSamples / audioSource.clip.frequency;
    }
    #region new timing system

    public void SmootherDSPTime()
    {
        double result = Time.unscaledTimeAsDouble * alphaValue + betaValue;
        if (result > currentSmoothDspTime)
        {
            currentSmoothDspTime = result;
        }
    }
    #region Linear regression
    /// <summary>
    /// Fits a line to a collection of (x,y) points.
    /// </summary>
    /// <param name="xVals">The x-axis values.</param>
    /// <param name="yVals">The y-axis values.</param>
    /// <param name="rSquared">The r^2 value of the line.</param>
    /// <param name="yIntercept">The y-intercept value of the line (i.e. y = ax + b, yIntercept is b).</param>
    /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
    public void LinearRegression(
        double[] xVals,
        double[] yVals,
        out double rSquared,
        out double yIntercept,
        out double slope)
    {
        if (xVals.Length != yVals.Length)
        {
            throw new Exception("Input values should be with the same length.");
        }

        double sumOfX = 0;
        double sumOfY = 0;
        double sumOfXSq = 0;
        double sumOfYSq = 0;
        double sumCodeviates = 0;

        for (var i = 0; i < xVals.Length; i++)
        {
            var x = xVals[i];
            var y = yVals[i];
            sumCodeviates += x * y;
            sumOfX += x;
            sumOfY += y;
            sumOfXSq += x * x;
            sumOfYSq += y * y;
        }

        var count = xVals.Length;
        var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
        //var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

        var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
        var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
        var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

        var meanX = sumOfX / count;
        var meanY = sumOfY / count;
        var dblR = rNumerator / Math.Sqrt(rDenom);

        rSquared = dblR * dblR;
        yIntercept = meanY - ((sCo / ssX) * meanX);
        slope = sCo / ssX;
        alphaValue = slope;
        betaValue = yIntercept;
    }
    #endregion
    #endregion
    #endregion
}