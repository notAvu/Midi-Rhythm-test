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
    public int ScoreCount;
    private SFXManager sfxManager;

    #region hitWindows Score. TODO: set a better score system that scales with timing/dificulty 
    public int GoodScore = 20;
    public double GoodScoreWindow = 0.1;
    public int NiceScore = 50;
    public double NiceScoreWindow = 0.05;
    public int PerfectScore = 100;
    public double PerfectScoreWindow = 0.025;
    #endregion
    void Start()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        ComboCount = 0;
    }
    public void NoteHit()
    {
        ComboCount++;
        ScoreCount += 50;
        onScoreChange?.Invoke();
        sfxManager.PlayHit();
    }
    public void NoteHit(double timeDiff)
    {
        ComboCount++;
        ScoreCalc(timeDiff);
        onScoreChange?.Invoke();
        sfxManager.PlayHit();
        //Instance.hitSFX.Play();
    }
    public void NoteMissed()
    {
        ComboCount = 0;
        onScoreChange?.Invoke();
        sfxManager.PlayMiss();
    }
    private void ScoreCalc(double timeDiff)
    {
        if (timeDiff > GoodScoreWindow)
        {
            NoteMissed();
        }
        else
        {
            ScoreCount += timeDiff <= PerfectScoreWindow ? PerfectScore : timeDiff <= NiceScoreWindow ? NiceScore : GoodScore; 
        }
        //else if (timeDiff <= GoodScoreWindow && timeDiff > NiceScoreWindow)
        //{
        //    ScoreCount += GoodScore;
        //}
        //else if (timeDiff <= NiceScoreWindow && timeDiff > PerfectScoreWindow)
        //{
        //    ScoreCount += NiceScore;
        //}
        //else if (timeDiff <= PerfectScoreWindow)
        //{
        //    ScoreCount += PerfectScore;
        //}
    }
}
