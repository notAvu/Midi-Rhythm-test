using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteScript : NoteScript
{
    public GameObject bar;
    private void Start()
    {
        Debug.Log(noteLength);
        bar.transform.localScale = new Vector2(transform.localScale.x,transform.localScale.y+noteLength);
    }
}
