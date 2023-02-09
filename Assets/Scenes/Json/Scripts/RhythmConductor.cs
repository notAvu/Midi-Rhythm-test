using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmConductor : MonoBehaviour
{
    public TextAsset jsonFile;
    private AudioSource songAudio;

    private int songBpm;
    private int notesPerBeat;
    private float secondsPerNote;//Should this be calculated for actual beats or chart beats (Subdivided)

    private double offset;
    private int columns;

    public float songPosition;
    public float songPositionSeconds;

    private float dpsTime;

    private void Awake()
    {
        ReadBeatmapInfo();
    }
    private void Start()
    {
        songAudio = GetComponent<AudioSource>();
        dpsTime = (float)AudioSettings.dspTime;
        secondsPerNote = 60f / (songBpm * notesPerBeat);
        //secondsPerBeat = 60f / songBpm ;
        songAudio.Play();
    }
    private void Update()
    {
        songPositionSeconds = (float)((AudioSettings.dspTime - dpsTime)*songAudio.pitch-offset);
        songPosition = songPositionSeconds / secondsPerNote;
    }
    private void ReadBeatmapInfo()
    {
        JsonBeatmapParser jsonParser = new JsonBeatmapParser();
        var beatmapInfo = jsonParser.ParseBeatmap(jsonFile);
        songBpm = beatmapInfo.BPM;
        notesPerBeat = beatmapInfo.notes[0].LPB;
        offset = beatmapInfo.offset;
        columns = beatmapInfo.maxBlock;
    }
}
