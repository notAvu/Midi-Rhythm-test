using System.Collections;
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
    [SerializeField] Image image = default;
    [SerializeField] Image imageLarge = default;
    [SerializeField] Button button = default;

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
        messageLarge.text = Index.ToString();
        var selected = Context.SelectedIndex == Index;
        //imageLarge.color = image.color = selected
        //    ? new Color32(0, 255, 255, 100)
        //    : new Color32(255, 255, 255, 77);
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
