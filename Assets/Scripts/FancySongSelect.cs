using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FancySongSelect : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float transitionDuration;
    [SerializeField]
    private GameObject prefab;
    //[SerializeField]
    //private GameObject scrollViewContent;
    private JsonBeatmapParser parser;
    [HideInInspector]
    public Song selectedSong;
    [HideInInspector]
    public SongDataContainer dataContainer;
    public AudioSource audioPlayer;
    private List<Song> songs;
    [SerializeField]
    private SongScroll songScrollView;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        parser = new JsonBeatmapParser();
        songs = new List<Song>();
        //SongCell.ClickPlay += GoToPlayScene;
        LoadSongs();
    }
    private void Start()
    {
        var selectedSongContainer = ScriptableObject.CreateInstance<SongDataContainer>();
        this.dataContainer = selectedSongContainer;
        audioPlayer = FindObjectOfType<AudioSource>();
    }
    private void LoadSongs()
    {
        var songFolderPath = Application.dataPath + "/Resources/Songs";
        DirectoryInfo i = new DirectoryInfo(songFolderPath);
        foreach (var dir in i.GetDirectories())
        {
            CreateSongItem(dir.Name);
        }
        songScrollView.UpdateData(songs);
        songScrollView.SelectCell(0);
    }
    private void CreateSongItem(string songDirName)
    {
        var newContainer = ScriptableObject.CreateInstance<SongDataContainer>();
        var json = Resources.Load($"Songs/{songDirName}/{songDirName}", typeof(TextAsset)) as TextAsset;
        var beatmapInfo = parser.ParseSong(json);
        //var songItem = Instantiate(prefab);
        var itemScript = new Song();
        itemScript.songDataContainer = newContainer;
        itemScript.songDataContainer.audioClip = Resources.Load<AudioClip>($"Songs/{songDirName}/{songDirName}");
        itemScript.songDataContainer.coverArt = Resources.Load<Sprite>($"Songs/{songDirName}/{songDirName}");
        itemScript.songDataContainer.beatmapJson = json;
        itemScript.songName = beatmapInfo.name;
        itemScript.bpm = beatmapInfo.BPM;
        itemScript.lanes = beatmapInfo.maxBlock;
        itemScript.songCoverImage = itemScript.songDataContainer.coverArt;
        songs.Add(itemScript);
        //songItem.gameObject.transform.SetParent(scrollViewContent.transform);
        itemScript.SelectedSongAction += SetSelectedSong;
    }
    private void SetSelectedSong(Song song)
    {
        StartCoroutine(TriggerFade(transitionDuration, song));
    }
    public void GoToPlayScene()
    {
        SceneManager.LoadScene("NoteParserTest");
        dataContainer = selectedSong.songDataContainer;
        dataContainer.audioClip = selectedSong.songDataContainer.audioClip;
        dataContainer.beatmapJson = selectedSong.songDataContainer.beatmapJson;
        dataContainer.coverArt = selectedSong.songDataContainer.coverArt;
        audioPlayer.Stop();
        audioPlayer = null;
    }
    private IEnumerator TriggerFade(float seconds, Song song)
    {
        audioPlayer.outputAudioMixerGroup.audioMixer.FindSnapshot("Off").TransitionTo(seconds);
        yield return new WaitForSeconds(seconds);
        if (selectedSong != null)
        {
            audioPlayer.Stop();
            Debug.Log("Now playing " + song);
        }
        selectedSong = song;
        audioPlayer.clip = song.songDataContainer.audioClip;
        audioPlayer.Play();
        audioPlayer.time = (audioPlayer.clip.length / 3);
        audioPlayer.outputAudioMixerGroup.audioMixer.FindSnapshot("On").TransitionTo(seconds + (seconds / 2));
    }
}
