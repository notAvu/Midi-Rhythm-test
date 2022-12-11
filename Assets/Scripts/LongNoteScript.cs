using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteScript : NoteScript
{
    //private double instantiationTimestamp;
    //private double timeSienceInstantiated;

    public GameObject bar;
    public GameObject endNote;
    private void Start()
    {
        //noteLength = noteLength / 64;
        Debug.Log(noteLength);
        //bar.transform.localScale = new Vector2(transform.localScale.x,transform.localScale.y+noteLength);
    }
    //void Update()
    //{
    //    //Debug.Log(assignedTime);
    //    timeSienceInstantiated = SongManager.GetAudioSourceTime() - instantiationTimestamp;
    //    float t = (float)(timeSienceInstantiated / (SongManager.Instance.NoteTime * 2));
    //    //Debug.Log("T: "+t);
    //    //Debug.Log("time since instantiated: " + timeSienceInstantiated+noteLength);
    //    if (t > 1)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        //interpolates note position between spawn and despawn points.
    //        transform.position = Vector3.Lerp(new Vector3(transform.position.x, SongManager.Instance.NoteSpawnPointY, 0), new Vector3(transform.position.x, SongManager.Instance.NoteDespawnY, 0), t);
    //    }
    //}
}
