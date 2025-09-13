using UnityEngine;
using Zenject;
using UnityEngine.Audio;
using TMPro;

public class MenuScript : MonoBehaviour
{

    AddressablesManager addressablesManager;
    GameManager gameManager;
    PlayerData playerData;

    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Obejcts")]
    public GameObject player;
    public GameObject enemyObject;


    public TMP_InputField enemyCounter;
    public Transform enemySpawnPoint;

    public AudioSource BGM;

    [Inject]
    void ZenjectSetup(AddressablesManager _addressablesManager, GameManager _gameManager, PlayerData _playerData)
    {
        addressablesManager = _addressablesManager;
        gameManager = _gameManager;
        playerData = _playerData;
    }

    public void NewGame()
    {
        BGM.Play();
        gameManager.waveCounter = 0;
        gameManager.killCounter = 0;
        gameManager.UpdateKillCounterText();
        playerData.hearthCounter = 2;
        playerData.UpdateHearts(2);
        addressablesManager.LoadAssets();
        menuPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        player.SetActive(true);
    }

    public void LoadGame()
    {
        BGM.Play();
        addressablesManager.LoadAssets();
        menuPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        player.SetActive(true);
    }

    public void MainMenu()
    {
        BGM.Stop();
        menuPanel.SetActive(true);
        pausePanel.SetActive(false);
        gamePanel.SetActive(false);
        player.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void StartStressTest()
    {
        BGM.Play();
        menuPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        player.SetActive(true);

        int enemyCount = 0;
        if (!string.IsNullOrEmpty(enemyCounter.text))
        {
            int.TryParse(enemyCounter.text, out enemyCount);
        }

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = enemySpawnPoint.position + new Vector3(
                Random.Range(-5f, 5f),
                0,
                Random.Range(-5f, 5f)
            );

            Instantiate(enemyObject, spawnPos, Quaternion.identity);
        }
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
