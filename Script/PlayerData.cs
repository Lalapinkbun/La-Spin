using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerData
{
    public string _name;
    public int _LCoin;

    //成就
    public Dictionary<int, Achievement> _achievementDict;

    public List<PlayerAchievementState> _achievementStates;
    

    public PlayerData(string name)
    {
        _name = name;
        _LCoin = 1;
        SetupAchievement();
    }

    public void SetupAchievement()
    {
        //無論如何都要覆蓋做成一個新的，在_achievementStates需要根據存檔覆蓋事宜則會透過GameDataManager.cs去做處理）
        _achievementDict = new Dictionary<int, Achievement>();
        _achievementStates = new List<PlayerAchievementState>();
        AddAchievement(0, "The First Roll", "do your first roll", "Hello World.");
        AddAchievement(1, "A Pair", "get the same shape on a 2-column slot", "We Are Couple One!");
        AddAchievement(2, "Triplet", "get the same shape on a 3-column slot", "Three of the Kind are not better than THE ROCKKKKKKK.");
        AddAchievement(3, "Four Times", "get the same shape on a 4-column slot", "How possible is it that 4 A's exists?");
        AddAchievement(4, "Jackpot!", "get the same shape on a 5-column slot", "How Did We Get Here?");
        AddAchievement(5, "Thousandaire", "get a total of 10,000 L Coins", "Almost, you need more 990,000 L Coins to become a millionaire lol");
        AddAchievement(6, "The Highest Possible", "get the highest L Coin without getting the same shape state", "Well, I guess it's still much huh?");
        AddAchievement(7, "The Lowest Possible", "get the lowest L Coin without getting the same shape on the 3-column slot or above", "bad luck bro.");
        AddAchievement(8, "Do Re Mi", "get Red Square, Blue Circle, Green Emerald in order on 3-column slot.", "Fa So La Ti DO!");
        AddAchievement(9, "Mi Re Do", "get Red Square, Blue Circle, Green Emerald in reverse order on 3-column slot.", "Ti La So Fa# MI");
        AddAchievement(10, "Full House", "get 3 same shape first or last and 2 shape first or last.", "pretty big huh?");
    }

    private void AddAchievement(int id, string name, string desc, string meme)
    {
        var ach = new Achievement(id, name, desc, meme);
        _achievementDict.Add(id, ach);
        _achievementStates.Add(new PlayerAchievementState(id));
    }

    public bool CheckAchievement(int id) => _achievementDict.ContainsKey(id);

    public bool IsPlayerHaveAchievementBefore(int id)
    {
        var state = _achievementStates.Find(s => s._id == id);
        return state != null && state._state;
    }

    public bool GettingAchievement(int id)
    {
        if (CheckAchievement(id))
        {
            var state = _achievementStates.Find(s => s._id == id);
            if (state != null && !state._state)
            {
                state._state = true;
                return true;
            }
        }
        return false;
    }

    public string GetAchievement(int id)
    {
        return _achievementDict[id].DebugAchievement();
    }

    public string GetAchievementName(int id)
    {
        return _achievementDict[id].GetName();
    }

    public string GetAchievementMemeDec(int id)
    {
        return _achievementDict[id].GetMemeDec();
    }

    public string GetAchievementDec(int id)
    {
        return _achievementDict[id].GetDescription();
    }
}

public class Achievement
{
    public int _id;
    private string _name;
    private string _description;
    private string _memeDec;

    public Achievement(int id, string name, string description, string memeDec)
    {
        _id = id;
        _name = name;
        _description = description;
        _memeDec = memeDec;
    }

    public string DebugAchievement()
    {
        return _name + " -> " + _description;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetMemeDec()
    {
        return _memeDec;
    }

    public string GetDescription()
    {
        return _description;
    }
}

[Serializable]
public class PlayerAchievementState
{
    public int _id;
    public bool _state;

    public PlayerAchievementState(int id)
    {
        _id = id;
        _state = false;
    }
}