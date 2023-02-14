using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject
{
    public NoteJson NoteInfo;
    public int LinesPerBeat;
    public int NoteIndex;
    public int Column;
    public NoteTypes Type;
    public List<NoteObject> nestedNotes;

    public NoteObject(NoteJson jsonData)
    {
        NoteInfo = jsonData;
        try
        {
            ConvertNoteInfo();
            if (Type == NoteTypes.LongNote)
            {
                ConvertNestedNotes();
            }
        }
        catch
        {
            Debug.Log("Couldn't parse JSON data to note data");
        }
    }

    private void ConvertNestedNotes()
    {
        nestedNotes = new List<NoteObject>();
        foreach (var nestedJson in NoteInfo.notes)
        {
            var nextNote = new NoteObject(nestedJson);
            nestedNotes.Add(nextNote);
        }
    }

    public void ConvertNoteInfo()
    {
        LinesPerBeat = NoteInfo.LPB;
        NoteIndex = NoteInfo.num;
        Column = NoteInfo.block;
        Type = NoteInfo.type switch
        {
            1 => NoteTypes.SingleHit,
            2 => NoteTypes.LongNote,
            _ => throw new System.NotImplementedException()
        };
    }
}
public enum HitWindow
{
    PERFECT,
    OK,
    ALMOST,
    MISS
}
