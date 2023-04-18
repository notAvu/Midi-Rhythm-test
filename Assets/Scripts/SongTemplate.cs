using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private void Start()
    {
        imageContainer.sprite = songCoverImage;
        gameObject.GetComponent<Image>().sprite = songCoverImage;
        songTitle.text = songName;
        songArtist.text = bpm.ToString();
    }
    public void SelectSong()
    {

    }
}
