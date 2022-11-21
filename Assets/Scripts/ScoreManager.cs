using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField]
    private AudioSource hitSFX;
    [SerializeField]
    private AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    private static int comboCount;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //set score on screen to current score
    }
    public void NoteHit()
    {
        comboCount++;
        Debug.Log($"combo count: {comboCount}");
        //Instance.hitSFX.Play();
    }
    public void NoteMissed()
    {
        comboCount = 0;
        Debug.Log("Miss");
        //Instance.missSFX.Play();
    }
}
