using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    public Button _buttonRight;
    public Button _buttonLeft;
    //public Animation _movingAnimation;
    //public AnimationClip _movingLeft;
    //public AnimationClip _movingRight;

    private int Page = 0;
    public int MaxPage = 1;

    //private bool isAnimating = false;

    public RectTransform _content;
    private bool _isMoving = false;
    public float _moveTime = 2f;
    private float _pageWidth = 6f;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _buttonLeft.interactable = (Page > 0);
        _buttonRight.interactable = (Page < MaxPage);
    }

    public void NextPage()
    {
        if (!_isMoving)
        {
            Page++;
            StartCoroutine(MovePage(Page));
            UpdateUI();
        }
    }

    public void PrevPage()
    {
        if (!_isMoving)
        {
            Page--;
            StartCoroutine(MovePage(Page));
            UpdateUI();
        }
    }

    IEnumerator MovePage(int page)
    {
        
        _isMoving = true;
        Vector2 startPos = _content.anchoredPosition;
        Vector2 targetPos = new Vector2(-page * _pageWidth, startPos.y);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / _moveTime;
            _content.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        _content.anchoredPosition = targetPos;
        _isMoving = false;

        /*
        _isMoving = true;
        float t = 6f;
        Vector2 startPosition = _content.localPosition;
        Vector2 endPosition = new Vector2(_content.localPosition.x + t, 0);

        while (t < 1)
        {
            t += Time.deltaTime / _moveTime;
            _content.localPosition = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        _isMoving = false;
        */
    }

    /*
    public void PageGoingRight()
    {
        if (Page < MaxPage && !isAnimating)
        {
            _movingAnimation.clip = _movingRight;
            _movingAnimation.Play();
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
            _movingAnimation.clip = _movingLeft;
            _movingAnimation.Play();
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
    */
}
