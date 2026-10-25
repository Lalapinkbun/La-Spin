using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerData
{
    public string _saveFileVersion;
    public string _name;
    public int _LCoin;

    public float _volumeBGM;
    public float _volumeSFX;

    //成就
    public Dictionary<int, Achievement> _achievementDict;

    public List<PlayerAchievementState> _achievementStates;
    

    public PlayerData(string name)
    {
        _name = name;
        _LCoin = 1;
        SetupAchievement();
        _volumeBGM = _volumeSFX = 1.00f;
    }

    public void SetupAchievement()
    {
        //無論如何都要覆蓋做成一個新的，在_achievementStates需要根據存檔覆蓋事宜則會透過GameDataManager.cs去做處理）
        _achievementDict = new Dictionary<int, Achievement>();
        _achievementStates = new List<PlayerAchievementState>();
        AddAchievement(0, "The First Roll", "do your first roll", "Hello World.");
        AddAchievement(1, "A Pair", "get the same shape on a 2-column slot", "We Are Couple One!");
        AddAchievement(2, "Triplet", "get the same shape on a 3-column slot", "Three of the Kind are not better than THE ROOKKKKKKK.");
        AddAchievement(3, "Four Times", "get the same shape on a 4-column slot", "How possible is it that 4 A's exists?");
        AddAchievement(4, "Jackpot!", "get the same shape on a 5-column slot", "How Did We Get Here?");
        AddAchievement(5, "Thousandaire", "get a total of 10,000 L Coins", "Almost, you need more 990,000 L Coins to become a millionaire lol");
        AddAchievement(6, "The Highest Possible", "get the highest L Coin without getting the same shape state", "Well, I guess it's still much huh?");
        AddAchievement(7, "The Lowest Possible", "get the lowest L Coin without getting the same shape on the 3-column slot or above", "bad luck bro.");
        AddAchievement(8, "Do Re Mi", "get Red Square, Blue Circle, Green Emerald in order on 3-column slot.", "Fa So La Ti DO!");
        AddAchievement(9, "Mi Re Do", "get Red Square, Blue Circle, Green Emerald in reverse order on 3-column slot.", "Ti La So Fa# MI");
        AddAchievement(10, "Full House", "get 3 same shape first or last and 2 shape first or last.", "pretty big huh?");
        AddAchievement(11, "Repetition", "get same shape 3 times in a row on 1-column slot.", "a draw again???");
        AddAchievement(12, "Penta Kill", "get same shape 5 times in a row on 1-column slot", "well, i guess on other hand, that mean you're somehow pretty luck huh? or... bad? idk.");
        AddAchievement(13, "Harderrrrr", "get same shape state total 2 times on 5-column slot.", "sooo lucky bro");
        AddAchievement(14, "Two Pairs", "get 2 same shpae first and 2 same shape last on 4-column slot.", "IT'S CALL SEVEN PAIRS BRO! oh yeah, this game can't even do 14-column slot.");
        AddAchievement(15, "Stalemate", "get same shape on 1st and 4th slot, and get other shape on 2nd and 3rd on 4-column slot.", "well, the most furthest distance in the world.");
        AddAchievement(16, "Pair King", "get same shape 3 times in a row on 2-column slot.", "I. WAS. A. KING.");
        AddAchievement(17, "151's", "get a total of 151 L Coins", "I Choose You!");
        AddAchievement(18, "3 + 1", "get 3 same shape first and other shape for last", "What was 9 + 10. 21?");
        AddAchievement(19, "Hundred Thousand Millionaire", "get a total of 100,000 L Coins", "yeah, you know that. you need more 900,000 L Coins to become a millionaire.");
        AddAchievement(20, "I dont know why I made this Achievement but this should be cool", "get Red Square, Blue Circle, Green Emerald, Blue Circle, Red Square in order on 5-column slot."
            , "About this thing, in short, you know that. so this thing... that's how it work, how? you guys can see and can see something on it. or maybe you dont get it, but that's what I meant to, if you dont know, you dont need to guess is, since that thing I see pretty long. so i only can say is, ya, just dont ask me anything cuz you know... ya. other else I can say this will be pretty profoundIt involves a lot of things you know. so if you know what I just say, you besically know it, if you dont know... maybe try to get it or, dont. so ya.");
        AddAchievement(21, "Lalabluebun", "get all Blue Circle on 4-column slot", "blue bone. orange bone.");
        Debug.Log($"總共 {_achievementStates.Count} 個成就");
    }

    private void AddAchievement(int id, string name, string desc, string meme)
    {
        var ach = new Achievement(id, name, desc, meme);
        _achievementDict.Add(id, ach);
        _achievementStates.Add(new PlayerAchievementState(id));
    }

    //檢查該id的成就存不存在
    public bool CheckAchievement(int id) => _achievementDict.ContainsKey(id);

    //回報目前該成就的情況
    public bool AchievementState(int id)
    {
        if (CheckAchievement(id))
        {
            var state = _achievementStates.Find(s => s._id == id);
            if (state != null)
            {
                return state._state;
            }
        }
        //如果什麽都找不到默認就已經獲得了，這樣就不會卡出什麽東西導致獲得成就（但後面的數據并沒有得到）
        return true;
    }

    //獲取成就
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