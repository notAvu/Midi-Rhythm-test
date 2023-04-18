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
        DirectoryInfo i = new DirectoryInfo(songFolderPath);
        var directoryList = Directory.GetDirectories(songFolderPath);
        foreach(var dir in i.GetDirectories())
        {
            var allres = Resources.LoadAll($"Songs/{dir.Name}");
            var songImage = Resources.Load<Sprite>($"Songs/{dir.Name}/{dir.Name}");
            var json = Resources.Load($"Songs/{dir.Name}/{dir.Name}", typeof(TextAsset)) as TextAsset;
            var songItem = Instantiate(prefab);
            var itemScript = songItem.GetComponent<SongTemplate>();
            itemScript.songCoverImage = (Sprite)songImage;
            itemScript.songName = dir.Name;
            songItem.gameObject.transform.parent = scrollViewContent.transform;
        }
        //itemScript.songName = info.Name;

    }
}
