using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public Button ButtonRight;
    public Button ButtonLeft;
    public Animation movingAnimation;
    public AnimationClip movingLeft;
    public AnimationClip movingRight;

    private int Page = 0;
    private int MaxPage = 1;
    private bool isAnimating = false;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        ButtonLeft.interactable = (Page > 0);
        ButtonRight.interactable = (Page < MaxPage);
    }

    public void PageGoingRight()
    {
        if (Page < MaxPage && !isAnimating)
        {
            movingAnimation.clip = movingRight;
            movingAnimation.Play();
            Page++;
            UpdateUI();
            StartCoroutine(CD());
        }
        else
        {
            Debug.LogWarning("已經是最後一頁了！");
        }
    }

    public void PageGoingLeft()
    {
        if (Page > 0 && !isAnimating)
        {
            movingAnimation.clip = movingLeft;
            movingAnimation.Play();
            Page--;
            UpdateUI();
            StartCoroutine(CD());
        }
        else
        {
            Debug.LogWarning("已經是第一頁了！");
        }
    }

    IEnumerator CD()
    {
        isAnimating = true;
        yield return new WaitForSeconds(0.3f);
        isAnimating = false;
    }
}
