using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Transition : MonoBehaviour
{
    public RectTransform _here;
    public Animation _transitionAnimation;
    public AnimationClip _transitionIn;
    public AnimationClip _transitionOut;

    private void Start()
    {
        _here = GetComponent<RectTransform>();
        // 場景一進來就先播 Out，把黑幕移走
        PlayOut();
    }

    public void PlayAndChangeScene(string sceneName)
    {
        StartCoroutine(PlayInAndLoad(sceneName));
    }

    private IEnumerator PlayInAndLoad(string sceneName)
    {
        _transitionAnimation.clip = _transitionIn;
        _transitionAnimation.Play();

        yield return new WaitForSeconds(_transitionIn.length + 0.1f);

        // 換場景
        FindAnyObjectByType<GameDataManager>().ChangeSpecificScene(sceneName);
    }

    private void PlayOut()
    {
        _transitionAnimation.clip = _transitionOut;
        _transitionAnimation.Play();
        _here.anchoredPosition = new Vector2(-2340f, 0);
    }
}
