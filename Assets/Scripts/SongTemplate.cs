using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SongTemplate : MonoBehaviour
{
    #region Song data
    [HideInInspector]
    public string songName;
    [HideInInspector]
    public int bpm;
    [HideInInspector]
    public int lanes;
    [HideInInspector]
    public AudioClip sound;
    [HideInInspector]
    public Sprite songCoverImage;
    #endregion
    #region UI components
    [SerializeField]
    private Image imageContainer;
    [SerializeField]
    private TextMeshProUGUI songTitle;
    [SerializeField]
    private TextMeshProUGUI songArtist;
    #endregion
    [SerializeField]
    public SongDataContainer songDataContainer;
    public event Action<SongTemplate> SelectedSongAction;
    private void Start()
    {
        gameObject.name = songName;
        songCoverImage = songDataContainer.coverArt;
        imageContainer.sprite = songCoverImage;
        gameObject.GetComponent<Image>().sprite = songCoverImage;
        songTitle.text = songName;
        songArtist.text = bpm.ToString();
    }
    public void SelectSong()
    {
        SelectedSongAction?.Invoke(this);
    }
}
