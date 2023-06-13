using System;
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
                instance = new GameObject().AddComponent<ScoreManager>();
                instance.name = "ScoreManager";
            }
            return instance;
        }
    }
    public Action onScoreChange;
    public int ComboCount;
    private SFXManager sfxManager;
    // Start is called before the first frame update
    void Start()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        ComboCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //set score on screen to current score
    }
    public void NoteHit()
    {
        ComboCount++;
        onScoreChange?.Invoke();
        //Debug.Log("Hit");
        //Instance.hitSFX.Play();
    }
    public void NoteMissed()
    {
        ComboCount = 0;
        onScoreChange?.Invoke();
        //Debug.Log("Miss");
        //Instance.missSFX.Play();
    }
}
