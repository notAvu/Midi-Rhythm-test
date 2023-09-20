using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleHitNote : HitNote, IHitObject
{
    public double InstantiationTimestamp;
    private double TimeSienceInstantiation;
    private void Update()
    {
        Debug.Log((RhythmConductor.Instance.songPositionSeconds - NoteTimestamp));
        TimeSienceInstantiation = RhythmConductor.Instance.GetAudioSourceTime() - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        if (t > 1)
        {
            Miss();
        }
        else if((RhythmConductor.Instance.GetAudioSourceTime()-NoteTimestamp) > 0)
        {
            //TODO 
        }
        else
        {
            transform.position = Vector2.Lerp(column.transform.position, column.despawnPosition, (float)t); 
        }
    }
    public void Hit()
    {
        column.InputIndex++;
        double timeDiff = Math.Abs(NoteTimestamp-RhythmConductor.Instance.songPositionSeconds);
        ScoreManager.Instance.NoteHit(timeDiff);
        Destroy(gameObject);
    }
    public void Miss()
    {
        column.InputIndex++;
        ScoreManager.Instance.NoteMissed();
        Destroy(gameObject);
    }
    #region Debug methods

    #endregion
}
