using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBar : MonoBehaviour
{
    [SerializeField]
    private static float lineThickness; 
    public static int LastIndex { get; set; }
    public int CurrentIndex;//This is actually a very lazy solution to avoid duplicated bars instantiating from rhythmConductor
    public float InstantiationTimestamp;
    private float TimeSienceInstantiation;
    public RhythmConductor conductor { private get; set; }
    private void Awake()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    //private void Start()
    //{
    //    //NoteTimestamp = InstantiationTimestamp;
    //    //transform.position = new Vector3(0, 6, 0);
    //}
    private void Update()
    {
        TimeSienceInstantiation = conductor.songPositionSeconds - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else if (conductor.lastBeat >= InstantiationTimestamp / conductor.secondsPerNote)
        {
            transform.position = Vector2.Lerp(new Vector2(0,6), new Vector2(0, -3), t);
        }
    }
}
