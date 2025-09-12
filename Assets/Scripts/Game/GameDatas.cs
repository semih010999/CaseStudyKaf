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

    [Inject]
    void ZenjectSetup(GameManager _gameManager, AddressablesManager _addressablesManager)
    {
        gameManager = _gameManager;
        addressablesManager = _addressablesManager;
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
    }

    public void SaveGameData()
    {
        GameData data = new GameData();
        ResetSaveImage().Forget();
        data.waveCounter = gameManager.waveCounter;
        data.killCounter = gameManager.killCounter;

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

            gameManager.UpdateKillCounterText();
            
        }
    }

    async UniTaskVoid ResetSaveImage()
    {
        await UniTask.Delay(2000);
        saveImageObject.SetActive(false);
    }
}