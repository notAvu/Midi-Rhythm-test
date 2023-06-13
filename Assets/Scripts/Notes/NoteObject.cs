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
            if (this.Type == NoteTypes.LongNote)
            {
                ConvertNestedNotes();
            }
        }
        catch
        {
            Debug.Log("Couldn't parse JSON data to note data");
        }
    }
    /// <summary>
    /// Gets the list of the note nodes that are children of the current note and creates a <seealso cref="NoteObject"/> list of them to add to the parent note's object
    /// </summary>
    private void ConvertNestedNotes()
    {
        nestedNotes = new List<NoteObject>();
        foreach (var nestedJson in NoteInfo.notes)
        {
            var nextNote = new NoteObject(nestedJson);
            nestedNotes.Add(nextNote);
        }
    }
    /// <summary>
    /// Converts the data from the <see cref="NoteInfo"/> to match this object's attributes
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    private void ConvertNoteInfo()
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

/*
 * This enum should be better implemented in a different file 
 */
public enum HitWindow
{
    PERFECT,
    OK,
    ALMOST,
    MISS
}
