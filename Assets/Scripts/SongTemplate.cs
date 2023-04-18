using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//[CreateAssetMenu(fileName = "New SongItem", menuName = "Song Item")]
public class SongTemplate : MonoBehaviour
{
    #region song data
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
    #region
    [SerializeField]
    private Image imageContainer;
    [SerializeField]
    private TextMeshProUGUI songTitle;
    [SerializeField]
    private TextMeshProUGUI songArtist;
    #endregion
    private SongSelectMenu menu;
    private const string SONG_LOADER_TAG = "SongLoader";
    public event Action<SongTemplate> selectedSongAction;
    private void Start()
    {
        gameObject.name = songName;
        menu = GameObject.FindWithTag(SONG_LOADER_TAG).GetComponent<SongSelectMenu>();
        imageContainer.sprite = songCoverImage;
        gameObject.GetComponent<Image>().sprite = songCoverImage;
        songTitle.text = songName;
        songArtist.text = bpm.ToString();
    }
    public void SelectSong()
    {
        selectedSongAction?.Invoke(this);
    }
}
