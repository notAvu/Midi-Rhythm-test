using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScoreManager();
            }
            return instance;
        }
    }
    [SerializeField]
    private AudioSource hitSFX;
    [SerializeField]
    private AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    private static int comboCount;
    // Start is called before the first frame update
    void Start()
    {
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
        //Debug.Log("Hit");
        //Debug.Log($"combo count: {comboCount}");
        //Instance.hitSFX.Play();
    }
    public void NoteMissed()
    {
        comboCount = 0;

        //Debug.Log("Miss");
        //Instance.missSFX.Play();
    }
}
