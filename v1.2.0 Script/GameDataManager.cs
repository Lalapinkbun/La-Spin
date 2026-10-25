using System.Diagnostics.CodeAnalysis;
using System.IO;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    private string CurrentVersion = "1.2.0";
    private PlayerData _playerData; 
    public PlayerDataObject _intexPlayerData;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (_playerData == null)
        {
            _playerData = new PlayerData("Player");
            _intexPlayerData._playerData._achievementDict = _playerData._achievementDict;
            //_intexPlayerData._playerData._achievementStates = _playerData._achievementStates;
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
        //名字
        if (playerData._name == null)
        {
            _intexPlayerData._playerData._name = _playerData._name;
        }
        else
        {
            _intexPlayerData._playerData._name = playerData._name;
        }
        //L Coin
        if (playerData._LCoin == 0)
        {
            _intexPlayerData._playerData._LCoin = _playerData._LCoin;
        }
        else
        {
            _intexPlayerData._playerData._LCoin = playerData._LCoin;
        }
        //成就字典
        _intexPlayerData._playerData._achievementDict = _playerData._achievementDict;
        //成就狀態
        if (playerData._achievementStates == null)
        {
            _intexPlayerData._playerData._achievementStates = _playerData._achievementStates;
        }
        else
        {
            _intexPlayerData._playerData._achievementStates = playerData._achievementStates;
        }
        //檢查更新
        UpdatePlayerData(playerData);
        //存檔版本號
        if (playerData._saveFileVersion == null)
        {
            _intexPlayerData._playerData._saveFileVersion = CurrentVersion;
        }
        else
        {
            if (playerData._saveFileVersion != CurrentVersion)
            {
                switch (playerData._saveFileVersion)
                {
                    default:
                        _intexPlayerData._playerData._saveFileVersion = CurrentVersion;
                        break;
                }
            }
            else
            {
                _intexPlayerData._playerData._saveFileVersion = playerData._saveFileVersion;
            }
        }
        //BGM
        _intexPlayerData._playerData._volumeBGM = playerData._volumeBGM;
        //SEF
        _intexPlayerData._playerData._volumeSFX = playerData._volumeSFX;
        //再存檔一遍以防萬一
        SavePlayerData();
    }

    public void UpdatePlayerData(PlayerData playerData)
    {
        string playerDataVersion = "0";
        if (playerData._saveFileVersion == null || playerData._saveFileVersion == "")
        {
            playerDataVersion = "1.1.0.1--"; // 記錄版本
        }
        else
        {
            playerDataVersion = playerData._saveFileVersion;
        }

        //如果版本對的上也就是不在對的版本會更新
        switch (playerDataVersion)
        {
            case "1.1.0.1--":
            case "1.1.0.1":
                for (int i = 11; i < 22; i++)
                {
                    if (_playerData._achievementStates[i] != null)
                    {
                        _intexPlayerData._playerData._achievementStates.Add(_playerData._achievementStates[i]);
                    }
                    else
                    {
                        Debug.Log($"嘗試新增成就 {i} 但資料庫不存在");
                    }
                }
                SavePlayerData();
                Debug.Log("已更新");
                break;
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
        _playerData._saveFileVersion = _intexPlayerData._playerData._saveFileVersion;
        _playerData._name = _intexPlayerData._playerData._name;
        _playerData._LCoin = _intexPlayerData._playerData._LCoin;
        _playerData._achievementStates = _intexPlayerData._playerData._achievementStates;
        _playerData._volumeBGM = _intexPlayerData._playerData._volumeBGM;
        _playerData._volumeSFX = _intexPlayerData._playerData ._volumeSFX;
    }


    public void CreateBackupSave(string name)
    {
        string defaultPath = Path.Combine(Application.streamingAssetsPath, "DefaultSave.json");
        string customPath = Path.Combine(Application.persistentDataPath, name + ".json");
        if (File.Exists(defaultPath))
        {
            string json = File.ReadAllText(defaultPath);
            File.WriteAllText(customPath, json);
            Debug.Log("已創建備份存檔。");
            LoadPlayerData();
        }
        else
        {
            Debug.LogError("找不到 DefaultSave.json，無法建立備份存檔！");
        }
    }

    public void DeleteSpecificFile(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
            Debug.Log("Deleted Save");
        }
        else
        {
            Debug.Log("找不到存档");
        }
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

    public void PlaySFX(int SFX, bool isLoop, bool isPlayOneShot)
    {
        AudioManager.Instance.PlaySFX(SFX, isLoop, isPlayOneShot);
    }

    public void StopSFX()
    {
        AudioManager.Instance.StopSFX();
    }

    public void UpdateVolumeBGM(float BGM)
    {
        _intexPlayerData._playerData._volumeBGM = BGM;
    }

    public void UpdateVolumeSFX(float SFX)
    {
        _intexPlayerData._playerData._volumeSFX = SFX;
    }
}
