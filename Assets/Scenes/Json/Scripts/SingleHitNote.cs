using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleHitNote : HitNote, IHitObject
{
    //TODO Get position and instantiation from lanes 
    public double InstantiationTimestamp;
    //public float NoteTimestamp;
    private double TimeSienceInstantiation;
    private void Awake()
    {
        //conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    private void Start()
    {
        //InstantiationTimestamp = (float)conductor.GetAudioSourceTime();
        //transform.position = new Vector3(0, 6, 0);
    }
    bool aux = true;
    private void Update()
    {
        TimeSienceInstantiation = RhythmConductor.Instance.GetAudioSourceTime() - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        //var aux = InstantiationTimestamp / conductor.secondsPerNote;
        if (t > 1)
        {
            if (aux)
            {
                aux = false;
                Miss();
            }
            //Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.Lerp(column.spawnPosition, column.despawnPosition, (float)t); //TODO:switch despawn position to hit position and then make it go from hitposition to spawnposition
        }
    }

    public void Hit()
    {
        column.InputIndex++;
        ScoreManager.Instance.NoteHit();
        Destroy(gameObject);
    }

    public void Miss()
    {
        column.InputIndex++;
        ScoreManager.Instance.NoteMissed();
        //Destroy(gameObject);
    }
    #region Debug methods

    #endregion
}
