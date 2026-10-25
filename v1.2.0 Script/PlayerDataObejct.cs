using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Unnamed-PlayerData", menuName = "PlayerData/CreateNewPlayerData")]
[Serializable]
public class PlayerDataObject : ScriptableObject
{
    public PlayerData _playerData;
}
