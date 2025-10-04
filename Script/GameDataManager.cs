using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    private PlayerData _playerData;
    public PlayerDataObject _intexPlayerData;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (_playerData == null)
        {
            _playerData = new PlayerData("Player");
            _intexPlayerData._playerData._achievementDict = _playerData._achievementDict;
        }
        // DontDestroyOnLoad(gameObject);
        /*
        for (int i = 0; i < _playerData._achievementDict.Count; i++)
        {
            Debug.Log(_playerData.GetAchievement(i).ToString());
        }
        */
    }

    public void Setup(PlayerData playerData)
    {
        if (playerData._name == null)
        {
            _intexPlayerData._playerData._name = _playerData._name;
        }
        else
        {
            _intexPlayerData._playerData._name = playerData._name;
        }

        if (playerData._LCoin == 0)
        {
            _intexPlayerData._playerData._LCoin = _playerData._LCoin;
        }
        else
        {
            _intexPlayerData._playerData._LCoin = playerData._LCoin;
        }

        _intexPlayerData._playerData._achievementDict = _playerData._achievementDict; 

        if (playerData._achievementStates == null)
        {
            _intexPlayerData._playerData._achievementStates = _playerData._achievementStates;
        }
        else
        {
            _intexPlayerData._playerData._achievementStates = playerData._achievementStates;
        } 
    }

    public void SavePlayerData()
    {
        UpdatePlayerDataFromRuntime();
        string json = JsonUtility.ToJson(_playerData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("玩家資料已儲存: " + SavePath);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            PlayerData loaded = JsonUtility.FromJson<PlayerData>(json);
            Setup(loaded);
            Debug.Log("玩家資料已讀取");
        }
        else
        {
            Debug.LogWarning("找不到存檔，可能是第一次遊玩。");
            CreateDefaultSave();
        }
    }

    private void UpdatePlayerDataFromRuntime()
    {
        if (_playerData == null) _playerData = new PlayerData("");
        _playerData._name = _intexPlayerData._playerData._name;
        _playerData._LCoin = _intexPlayerData._playerData._LCoin;
        _playerData._achievementStates = _intexPlayerData._playerData._achievementStates;
    }

    private void CreateDefaultSave()
    {
        string defaultPath = Path.Combine(Application.streamingAssetsPath, "DefaultSave.json");
        if (File.Exists(defaultPath))
        {
            string json = File.ReadAllText(defaultPath);
            File.WriteAllText(SavePath, json);
            Debug.Log("已創建初始存檔。");
            LoadPlayerData();
        }
        else
        {
            Debug.LogError("找不到 DefaultSave.json，無法建立初始存檔！");
        }
    }

    public void DeletePlayerSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Deleted Save");
        }
        else
        {
            Debug.Log("找不到存档");
        }
    }

    public void ChangeSpecificScene(string Scene)
    {
        if (Scene != null)
        {
            SceneManager.LoadScene(Scene);
        }
        else
        {
            Debug.LogWarning("NextScene is not assigned!");
        }
    }


    public void MakeTransition(string Scene)
    {
        if (Scene != null)
        {
            FindAnyObjectByType<Transition>().PlayAndChangeScene(Scene);
        }
    }

    public void ChangeBGM(int BGM)
    {
        AudioManager.Instance.PlayMusic(BGM);
    }
}
