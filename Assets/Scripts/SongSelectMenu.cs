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
    private void Awake()
    {
        LoadSongs();
    }
    private void LoadSongs()
    {
        var songFolderPath = Application.dataPath + "/Resources/Songs";
        //DirectoryInfo i = new DirectoryInfo(songFolderPath);
        var directoryList = Directory.GetDirectories(songFolderPath);
        foreach(var dir in directoryList)
        {
            Debug.Log(dir);
        }
        //var songItem = Instantiate(prefab);
        //songItem.gameObject.transform.parent = scrollViewContent.transform;
        //var itemScript = songItem.GetComponent<SongTemplate>();
        //itemScript.songName = info.Name;

    }
}
