using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHitNote : MonoBehaviour
{
    NoteObject hitObject;
    public float InstantiationTimestamp;
    public float NoteTimestamp;
    private float TimeSienceInstantiation;
    RhythmConductor conductor;
    public int index;
    public static int lastIndex;
    private void Awake()
    {
        conductor = GameObject.Find("RhythmConductor").GetComponent<RhythmConductor>();
    }
    private void Start()
    {
        NoteTimestamp = InstantiationTimestamp;
        transform.position = new Vector3(0, 6, 0);
    }
    private void Update()
    {
        TimeSienceInstantiation = conductor.songPositionSeconds - InstantiationTimestamp;
        var t = TimeSienceInstantiation;
        Debug.Log(t);
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.Lerp(new Vector2(gameObject.transform.position.x, 10), new Vector2(gameObject.transform.position.x, -6), t);
        }
    }
}
