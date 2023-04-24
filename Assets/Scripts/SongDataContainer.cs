using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SongData", menuName = "Song Data")]
public class SongDataContainer : ScriptableObject
{
    public AudioClip audioClip;
    public TextAsset beatmapJson;
    public Sprite coverArt;
}
