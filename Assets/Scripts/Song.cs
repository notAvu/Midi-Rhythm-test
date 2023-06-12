using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song 
{
    #region Song data
    public string songName;
    public int bpm;
    public int lanes;
    public AudioClip sound;
    public Sprite songCoverImage;
    #endregion
    //[SerializeField]
    public SongDataContainer songDataContainer;
    public event Action<Song> SelectedSongAction;
    public Song()
    {
        //songCoverImage = songDataContainer.coverArt;
        //songName = songDataContainer.
    }
    public Song(SongDataContainer container)
    {
        songDataContainer = container;
        songCoverImage = songDataContainer.coverArt;
    }
    public void SelectSong()
    {
        SelectedSongAction?.Invoke(this);
    }
}
