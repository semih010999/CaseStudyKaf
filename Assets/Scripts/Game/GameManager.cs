using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Zenject;
using System.Threading;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text waveText;
    public TMP_Text nextWaveTimerText;
    public TMP_Text killCounterText;
    public TMP_Text infoText;

    public TMP_Text fpsText;
    float deltaTime;

    [Header("Values")]
    public List<Transform> waypoints;
    public int waveCounter = 0;
    public List<int> waveEnemyCounterList;
    public int waveEnemyCounter = 0;
    public bool isStartWave = false;
    public int killCounter;
    AddressablesManager addressablesManager;
    GameDatas gameDatas;
    CancellationTokenSource cts;

    [Inject]
    void ZenjectSetup(AddressablesManager _addressablesManager, GameDatas _gameDatas)
    {
        addressablesManager = _addressablesManager;
        gameDatas = _gameDatas;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            cts?.Cancel();
            NextWave();
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        fpsText.text = $"FPS: {fps:0}";
    }

    public void UpdateWaveText()
    {
        waveText.text = $"Wave {waveCounter + 1}";
    }

    public void UpdateKillCounterText()
    {
        killCounterText.text = $"Kill: {killCounter}";
    }

    void NextWave()
    {
        addressablesManager.LoadAssets();
        nextWaveTimerText.text = "";
        infoText.text = "";
    }

    public void StartNextWaveTimer(int duration)
    {
        gameDatas.SaveGameData();
        cts?.Cancel();
        cts = new CancellationTokenSource();

        NextWaveTimer(duration, cts.Token).Forget();
    }

    async UniTaskVoid NextWaveTimer(int duration, CancellationToken token)
    {
        for (int t = duration; t >= 0; t--)
        {
            int minutes = t / 60;
            int seconds = t % 60;

            nextWaveTimerText.text = $"Next wave in: {minutes:00}:{seconds:00}";
            infoText.text = "Press the Z key to call the next wave early!";

            try
            {
                await UniTask.Delay(1000, cancellationToken: token);
            }
            catch
            {
                return;
            }

            if (t == 0)
                NextWave();
        }
    }
}
