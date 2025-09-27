using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private PlayerData _playerData;
    public PlayerDataObject _intexPlayerData;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (_playerData == null)
            _playerData = new PlayerData("Player");
        // DontDestroyOnLoad(gameObject);
    }

    public void Setup(PlayerData playerData)
    {
        _intexPlayerData._playerData._name = playerData._name;
        _intexPlayerData._playerData._LCoin = playerData._LCoin;
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
}
