using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    private double instantiationTimestamp; //still have to figure aut how to label this better so it looks less confusing
    private float assignedTime;
    private double timeSienceInstantiated;

    void Start()
    {
        instantiationTimestamp = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        timeSienceInstantiated = SongManager.GetAudioSourceTime() - instantiationTimestamp;
        float t = (float)(timeSienceInstantiated / (SongManager.Instance.NoteTime * 2));
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //interpolates note position between spawn and despawn points.
            transform.position = Vector3.Lerp(Vector3.up * SongManager.Instance.NoteSpawnPointY, Vector3.up * SongManager.Instance.NoteDespawnY, t); 
        }
    }
}
