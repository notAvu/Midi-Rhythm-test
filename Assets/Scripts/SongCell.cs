using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;
using FancyScrollView.Example03;

class SongCell : FancyCell<Song, Context>
{
    [SerializeField] Animator animator = default;
    [SerializeField] Text message = default;
    [SerializeField] Text messageLarge = default;
    [SerializeField] Text bpm;
    [SerializeField] Text difficulty;//PlaceHolder
    [SerializeField] Image image = default;
    [SerializeField] Image imageLarge = default;
    [SerializeField] Button button = default;
    [SerializeField] Button playBtn;
    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }
    void Start()
    {
        button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
    }
    public override void UpdateContent(Song itemData)
    {
        message.text = itemData.songName;
        messageLarge.text = itemData.songName;
        bpm.text = itemData.bpm.ToString();
        var selected = Context.SelectedIndex == Index;
        //imageLarge.color = image.color = selected
        //    ? new Color32(255, 255, 255, 100)
        //    : new Color32(105, 255, 255, 12);
        image.sprite = itemData.songCoverImage;
        imageLarge.sprite = itemData.songCoverImage;
    }
    public override void UpdatePosition(float position)
    {
        currentPosition = position;
        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }
        animator.speed = 0;
    }
    float currentPosition = 0;
    void OnEnable() => UpdatePosition(currentPosition);
}
