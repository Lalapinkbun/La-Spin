using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string _name;
    public int _LCoin;

    public PlayerData(string name)
    {
        _name = name;
        _LCoin = 1;
    }
}
