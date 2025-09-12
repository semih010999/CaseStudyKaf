using UnityEngine;
using Zenject;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{

    AddressablesManager addressablesManager;

    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;

    [Header("Obejcts")]
    public GameObject player;

    public AudioSource BGM;

    [Inject]
    void ZenjectSetup(AddressablesManager _addressablesManager)
    {
        addressablesManager = _addressablesManager;
    }

    public void StartGame()
    {
        BGM.Play();
        addressablesManager.LoadAssets();
        menuPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        player.SetActive(true);
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
