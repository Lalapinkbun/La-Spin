using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] _fullOst; // 所有 OST

    [SerializeField] private float fadeDuration = 1.5f;

    public int _currentOst = 0; // 紀錄目前的 OST index

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (_currentOst != -1)
            {
                ChangeToNewClip(_fullOst[_currentOst]);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int ostNum)
    {
        if (ostNum < 0 || ostNum >= _fullOst.Length) return;
        if (_currentOst == ostNum) return; // 已經在播這首，不切換
        if (ostNum == -1) bgmSource.clip = null;

        StartCoroutine(FadeToNewClip(_fullOst[ostNum]));
        _currentOst = ostNum;
    }

    private void ChangeToNewClip(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }


    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        float startVolume = bgmSource.volume;

        //先fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        bgmSource.Stop();

        //然後換新音樂
        bgmSource.clip = newClip;
        bgmSource.Play();

        //在fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        bgmSource.volume = startVolume;
    }
}
