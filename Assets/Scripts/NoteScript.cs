using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    Animator animator;
    protected double instantiationTimestamp; //still have to figure aut how to label this better so it looks less confusing
    public float assignedTime;
    protected double timeSienceInstantiated;
    [HideInInspector]
    public float noteLength;

    void Start()
    {
        instantiationTimestamp = SongManager.GetAudioSourceTime();
        animator = GetComponent<Animator>();
        //noteLength = noteLength / 64;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(assignedTime);
        timeSienceInstantiated = SongManager.GetAudioSourceTime() - instantiationTimestamp;
        float t = (float)(timeSienceInstantiated / (SongManager.Instance.NoteTime * 2));
        //Debug.Log("T: "+t);
        //Debug.Log("time since instantiated: " + timeSienceInstantiated+noteLength);
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //interpolates note position between spawn and despawn points.
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, SongManager.Instance.NoteSpawnPointY, 0), new Vector3(transform.position.x, SongManager.Instance.NoteDespawnY, 0), t);
        }
    }
    public void NoteHit()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit");
            StartCoroutine("HitAnimation");
        }
    }
    private IEnumerator HitAnimation()
    {
        float animationDuration =
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        yield return new WaitForSeconds(animationDuration);
        Destroy(this.gameObject);
    }
}
