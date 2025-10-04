using UnityEngine;

public class AchievementList : MonoBehaviour
{
    public GameObject AchiSlot;
    private PlayerData playerData;
    public PlayerDataObject playerDataObject;

    private Vector2 startPosition = new Vector2(-450, 0);
    private int spacing = 740;

    private void Awake()
    {
        playerData = new PlayerData("Achi");
        Setup();
    }

    public void Setup()
    {
        for (int i = 0; i < playerData._achievementDict.Count; i++)
        {
            GameObject clone = Instantiate(AchiSlot, transform);
            RectTransform rect = clone.GetComponent<RectTransform>();
            rect.anchoredPosition = startPosition + new Vector2(spacing * i, 0);

            AchievementSlot slot = clone.GetComponent<AchievementSlot>();
            slot.SetupSlot(
                playerData._achievementDict[i].GetName(),
                playerData._achievementDict[i].GetDescription(),
                playerData._achievementDict[i].GetMemeDec(),
                playerDataObject._playerData._achievementStates[i]._state
            );
        }
    }
}
