using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonBeatmapParser
{
    //public string fileLocation;
    public List<NoteJson> notes;
    public BeatmapJson parsedBeatmap;
    public JsonBeatmapParser()
    {
        notes = new List<NoteJson>();
    }
    public string JsonToText(string jsonFilePath)
    {
        var jsonContent = Resources.Load<TextAsset>(jsonFilePath);
        return jsonContent.text;
    }
    public BeatmapJson ParseBeatmap(TextAsset jsonFile)
    {
        parsedBeatmap = JsonUtility.FromJson<BeatmapJson>(jsonFile.text);
        foreach (var note in parsedBeatmap.notes)
        {
            notes.Add(note);
        }
        return parsedBeatmap;
    }
    public BeatmapJson ParseSong(TextAsset jsonFile)
    {
        return JsonUtility.FromJson<BeatmapJson>(jsonFile.text);
    }
    public BeatmapJson ParseBeatmap(string jsonText)
    {
        parsedBeatmap = JsonUtility.FromJson<BeatmapJson>(jsonText);
        foreach (var note in parsedBeatmap.notes)
        {
            notes.Add(note);
        }
        return parsedBeatmap;
    }
}
public enum NoteTypes
{
    SingleHit = 1,
    LongNote = 2
}
[System.Serializable]
public class NoteJson
{
    public int LPB;
    public int num;
    public int block;
    public int type;
    public NoteJson[] notes;
}
[System.Serializable]
public class BeatmapJson
{
    public string name;
    public int maxBlock;
    public int BPM;
    public double offset;
    public NoteJson[] notes;
}