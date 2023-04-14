using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SongItem", menuName = "Song Item")]
public class SongTemplate : ScriptableObject
{
    public string songName;
    public string songArtist;

    public int bpm;

    public AudioClip sound;

    public Sprite songCoverImage;

}
