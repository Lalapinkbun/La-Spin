using UnityEngine;
using UnityEngine.UI;

public class SlotLimter : MonoBehaviour
{
    public Button[] LevelButton;
    private int[] Limit = new int[]
    {
        0,
        20,
        100,
        200,
        500
    };
    public GameObject[] Warning;
    public PlayerDataObject PlayerData;

    public void Awake()
    {
        if (PlayerData != null)
        {
            for (int i = 0; i < Limit.Length; i++)
            {
                if (PlayerData._playerData._LCoin >= Limit[i])
                {
                    LevelButton[i].interactable = true;
                    Warning[i].SetActive(false);
                }
                else
                {
                    LevelButton[i].interactable = false;
                    Warning[i].SetActive(true);
                }
            }
        }
    }

}
