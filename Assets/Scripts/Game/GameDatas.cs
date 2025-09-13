using System.IO;
using UnityEngine;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Zenject;

public class GameDatas : MonoBehaviour
{
    public static GameDatas Instance;
    private string saveFilePath;
    public GameObject saveImageObject;
    GameManager gameManager;
    AddressablesManager addressablesManager;
    PlayerData playerData;

    [Inject]
    void ZenjectSetup(GameManager _gameManager, AddressablesManager _addressablesManager, PlayerData _playerData)
    {
        gameManager = _gameManager;
        addressablesManager = _addressablesManager;
        playerData = _playerData;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        LoadGameData();
    }

    [System.Serializable]
    public class GameData
    {
        public int waveCounter = 0;
        public int killCounter;
        public int hearthCounter = 2;
    }

    public void SaveGameData()
    {
        GameData data = new GameData();
        saveImageObject.SetActive(true);
        ResetSaveImage().Forget();
        data.waveCounter = gameManager.waveCounter;
        data.killCounter = gameManager.killCounter;
        data.hearthCounter = playerData.hearthCounter;

        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
    }

    public void LoadGameData()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            gameManager.waveCounter = data.waveCounter;
            gameManager.killCounter = data.killCounter;
            playerData.hearthCounter = data.hearthCounter;

            playerData.UpdateHearts(playerData.hearthCounter);
            gameManager.UpdateKillCounterText();
            
        }
    }

    async UniTaskVoid ResetSaveImage()
    {
        await UniTask.Delay(2000);
        saveImageObject.SetActive(false);
    }
}