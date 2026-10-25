using UnityEngine;
using UnityEngine.UI;

public class AchievementUIController : MonoBehaviour
{
    public Animation popUpAnimation;

    public Image picImage;
    public Text theName;
    public Text theMeme;

    public void AchievementAnimation(string name, string meme)
    {
        if (popUpAnimation.isPlaying)
        {
            popUpAnimation.Stop();
            theName.text = name;
            theMeme.text = meme;
            popUpAnimation.Play();
        }
        theName.text = name;
        theMeme.text = meme;
        popUpAnimation.Play();
    }
}
