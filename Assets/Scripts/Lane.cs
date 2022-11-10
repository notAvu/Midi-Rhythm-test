using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.MusicTheory;

public class Lane : MonoBehaviour
{
    [SerializeField]
    private NoteName musicalNote; //Lanes are asinged to each note of the piano roll so, for example G notes are a lane, A# notes are another lane
    [SerializeField]
    private KeyCode inputButton;

    public GameObject NotePrefab;
    List<Note> notes;
    private List<double> noteTimestamps;

    int spawnIndex = 0;
    int inputIndex = 0;

    void Start()
    {
        notes = new List<Note>();
        noteTimestamps = new List<double>();
    }

    void Update()
    {
        
    }
}
