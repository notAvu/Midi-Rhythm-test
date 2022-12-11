using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteScript : NoteScript
{
    //private double instantiationTimestamp;
    //private double timeSienceInstantiated;

    //public GameObject bar;
    public GameObject endNote;
    private void Start()
    {
        endNote.transform.position = new Vector2(transform.position.x, transform.position.y + noteLength);
        Debug.Log(noteLength);
        //bar.transform.localScale = new Vector2(transform.localScale.x,transform.localScale.y+noteLength);
    }
    private void Update()
    {
        //endNote.transform.position = new Vector2(transform.position.x, transform.position.y + noteLength);

        timeSienceInstantiated = SongManager.GetAudioSourceTime() - instantiationTimestamp;
        float t = (float)(timeSienceInstantiated / (SongManager.Instance.NoteTime * 2));
        if (t > 1 )
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, SongManager.Instance.NoteSpawnPointY, 0), new Vector3(transform.position.x, SongManager.Instance.NoteDespawnY, 0), t);
        }

    }
}
