using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SongUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI comboCountText;

    private void Start()
    {
        ScoreManager.Instance.onScoreChange += UpdateScore;
    }
    private void UpdateScore()
    {
        comboCountText.text = $"Combo: {ScoreManager.Instance.ComboCount}";
    }
}
