using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public NoteJson noteInfo;
    public int linesPerBeat;
    public int noteIndex;
    public int column;
    public NoteTypes type;
    public double NoteTimestamp { get; set; }
    public void ConvertNoteInfo()
    {
        linesPerBeat = noteInfo.LPB;
        noteIndex = noteInfo.num;
        column = noteInfo.block;
        type = noteInfo.type switch
        {
            1 => NoteTypes.SingleHit,
            2 => NoteTypes.LongNote,
            _ => throw new System.NotImplementedException()
        };
    }
    //private void ParseNoteType(int typeIndex)
    //{
        
    //}
}
public enum HitWindow
{
    PERFECT,
    OK,
    MEH,
    MISS
}
