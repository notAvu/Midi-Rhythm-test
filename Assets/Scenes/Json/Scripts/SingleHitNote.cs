using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHitNote : MonoBehaviour
{
    //TODO Get position and instantiation from lanes 
    public NoteObject noteData;
    public float InstantiationTimestamp;
    public float NoteTimestamp;
    private float TimeSienceInstantiation;
    public SpawnColumn column;
    public RhythmConductor conductor { private get; set; } 
    private void Awake()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    private void Start()
    {
        //NoteTimestamp = InstantiationTimestamp;
        //transform.position = new Vector3(0, 6, 0);
    }
    private void Update()
    {
        TimeSienceInstantiation = conductor.songPositionSeconds - this.InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        Debug.Log(t);
        var aux = InstantiationTimestamp / conductor.secondsPerNote;
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else /*if(conductor.lastBeat>= aux)*/
        {
            transform.position = Vector2.Lerp(column.spawnPosition,column.despawnPosition, t);
        }
    }
}
