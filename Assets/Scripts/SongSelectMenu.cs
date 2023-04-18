using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// TODO: set this as a singleton
/// </summary>
public class SongSelectMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject scrollViewContent;
    private JsonBeatmapParser parser;

    public SongTemplate selectedSong;
    public AudioSource audioPlayer;
    private void Awake()
    {
        parser = new JsonBeatmapParser();
        LoadSongs();
    }
    private void LoadSongs()
    {
        var songFolderPath = Application.dataPath + "/Resources/Songs";
        DirectoryInfo i = new DirectoryInfo(songFolderPath);
        foreach (var dir in i.GetDirectories())
        {
            CreateSongItem(dir.Name);
        }
    }
    private void CreateSongItem(string songDirName)
    {
        var songImage = Resources.Load<Sprite>($"Songs/{songDirName}/{songDirName}");
        var json = Resources.Load($"Songs/{songDirName}/{songDirName}", typeof(TextAsset)) as TextAsset;
        var beatmapInfo = parser.ParseSong(json);
        var songItem = Instantiate(prefab);
        var itemScript = songItem.GetComponent<SongTemplate>();
        itemScript.songCoverImage = songImage;
        itemScript.songName = beatmapInfo.name;
        itemScript.bpm = beatmapInfo.BPM;
        itemScript.lanes = beatmapInfo.maxBlock;
        songItem.gameObject.transform.SetParent(scrollViewContent.transform);
        itemScript.selectedSongAction += SetSelectedSong;
    }
    private void SetSelectedSong(SongTemplate song)
    {
        if (selectedSong != null)
        {
            audioPlayer.Stop();
            Debug.Log("Now playing " + selectedSong.name);
        }
        selectedSong = song;
        audioPlayer.clip = Resources.Load<AudioClip>($"Songs/{song.name}/{song.name}");
        audioPlayer.time = (audioPlayer.clip.length / 3);
        audioPlayer.Play();
    }
}
