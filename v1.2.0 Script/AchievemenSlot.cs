using UnityEngine;
using UnityEngine.UI;

public class AchievementSlot : MonoBehaviour
{
    //public static AchievementSlot Instance;
    public Text _name;
    public Text _description;
    public Text _meme;
    public Image _markDone;

    private void Awake()
    {
        //Instance = this;
    }

    public void SetupSlot(string name, string description, string meme, bool isdone)
    {
        _name.text = name;
        _description.text = description;
        _meme.text = meme;
        if (isdone)
        {
            _markDone.gameObject.SetActive(true);
        }
        else
        {
            _markDone.gameObject.SetActive(false);
        }
    }
}
