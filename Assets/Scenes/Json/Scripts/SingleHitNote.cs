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
        DrawIndex();
    }
    private void Update()
    {
        TimeSienceInstantiation = (float)RhythmConductor.Instance.GetAudioSourceTime() - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        //Debug.Log(t);
        if(noteData.NoteIndex == 110)
        {
            //Debug.Log(gameObject.GetComponent<IHitObject>().GetType().Name);
        }
        //var aux = InstantiationTimestamp / conductor.secondsPerNote;
        if (t > 1)
        {
            Destroy(gameObject);
            Miss();
        }
        else 
        {
            transform.position = Vector2.Lerp(column.spawnPosition,column.despawnPosition, t); //TODO:switch despawn position to hit position and then make it go from hitposition to spawnposition
        }
    }

    public void Hit()
    {
        column.InputIndex++;
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
        s.text = $"{noteData.NoteIndex}";
    }
    #endregion
}
