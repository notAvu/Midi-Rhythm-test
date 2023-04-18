using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SongSelectMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject scrollViewContent;
    private JsonBeatmapParser parser;
    [SerializeField]
    private AudioSource audioSource;
    private void Awake()
    {
        parser = new JsonBeatmapParser();
        LoadSongs();
    }
    private void LoadSongs()
    {
        var songFolderPath = Application.dataPath + "/Resources/Songs";
        DirectoryInfo i = new DirectoryInfo(songFolderPath);
        foreach(var dir in i.GetDirectories())
        {
            CreateSongItem(dir.Name);
        }
    }
    private void CreateSongItem(string songDirName)
    {
        var songImage = Resources.Load<Sprite>($"Songs/{songDirName}/{songDirName}");
        var json = Resources.Load($"Songs/{songDirName}/{songDirName}", typeof(TextAsset)) as TextAsset;
        var beatmapInfo = parser.ParseBeatmap(json);
        var songItem = Instantiate(prefab);
        var itemScript = songItem.GetComponent<SongTemplate>();
        itemScript.songCoverImage = songImage;
        itemScript.songName = beatmapInfo.name;
        itemScript.bpm = beatmapInfo.BPM;
        itemScript.lanes = beatmapInfo.maxBlock;
        songItem.gameObject.transform.SetParent(scrollViewContent.transform);
    }
}
