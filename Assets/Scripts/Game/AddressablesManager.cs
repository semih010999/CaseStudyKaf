using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;
using Zenject;

public class AddressablesManager : MonoBehaviour
{
    public static AddressablesManager instance;
    GameManager gameManager;
    PlayerAttack playerAttack;

    [Header("Asset References")]
    [SerializeField] List<AssetReferenceGameObject> levelReference;
    [SerializeField] AssetLabelReference assetReferenceLabel;
    [SerializeField] GameObject spawnedObject;
    public Transform spawnParent;


    [Inject] DiContainer _container;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    [Inject]
    void ZenjectSetup(GameManager _gameManager, PlayerAttack _playerAttack)
    {
        gameManager = _gameManager;
        playerAttack = _playerAttack;
    }

    public void LoadAssets()
    {
        if(spawnedObject != null)
        {
            ReleaseAssets(gameManager.waveCounter - 1);
        }
        levelReference[gameManager.waveCounter].InstantiateAsync(spawnParent).Completed += 
        (asyncOperationHandle) => 
        {
            spawnedObject = asyncOperationHandle.Result;
            _container.InjectGameObject(spawnedObject);
            gameManager.waveEnemyCounter = gameManager.waveEnemyCounterList[gameManager.waveCounter];
            gameManager.isStartWave = true;
            gameManager.UpdateWaveText();
        };
    }

    public void ReleaseAssets(int levelIndex)
    {
        levelReference[levelIndex].ReleaseInstance(spawnedObject);
    }
}