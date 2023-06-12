using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource hitSFX;
    [SerializeField]
    private AudioSource missSFX;
    public void PlayMiss()
    {
        missSFX.Play();
    }
    public void PlayHit()
    {
        hitSFX.Play();
    }
}
