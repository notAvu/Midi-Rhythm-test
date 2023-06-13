using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class that contains common data for different types of hittable objects (Single hit notes, Long notes, and possible future note types) <br/><br/>
/// <b>TODO: should I make it implement <seealso cref="IHitObject"/>?</b>
/// </summary>
public class HitNote : MonoBehaviour
{
    public NoteObject noteData;
    public SpawnColumn column;
    public double NoteTimestamp;
}
