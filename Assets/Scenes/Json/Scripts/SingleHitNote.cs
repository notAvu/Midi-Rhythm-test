using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleHitNote : HitNote, IHitObject
{
    //TODO Get position and instantiation from lanes 
    public float InstantiationTimestamp;
    //public float NoteTimestamp;
    private float TimeSienceInstantiation;
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
        DrawIndex();
        TimeSienceInstantiation = (float)RhythmConductor.Instance.GetAudioSourceTime() - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        //Debug.Log(t);
        if (Mathf.Approximately(NoteTimestamp, RhythmConductor.Instance.songPositionSeconds))
        {
            s.text = $"HitTime";
        }
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
            transform.position = Vector2.Lerp(column.spawnPosition, column.despawnPosition, t); //TODO:switch despawn position to hit position and then make it go from hitposition to spawnposition
        }
    }

    public void Hit()
    {
        column.InputIndex++;
        Destroy(gameObject);
    }

    public void Miss()
    {
        column.InputIndex++;
    }
    #region Debug methods
    [SerializeField]
    private TextMeshProUGUI s;
    private void DrawIndex()
    {
        s.text = $"C_I_{column.InputIndex}";
    }
    #endregion
}
