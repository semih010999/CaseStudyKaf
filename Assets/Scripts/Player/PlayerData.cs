using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Zenject;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    PlayerMovement playerMovement;
    MenuScript menuScript;
    public bool isDamageable = true;
    
    public int health = 100;
    public int maxHealth = 100;
    public int hearthCounter = 2;
    public Transform spawnPoint;
    public Transform playerTransform;
    public GameObject shieldObject;
    public Image heathBar;
    public List<GameObject> hearts;

    [Inject]
    void ZenjectSetup(PlayerMovement _playerMovement, MenuScript _menuScript)
    {
        playerMovement = _playerMovement;
        menuScript = _menuScript;
    }

    public void TakeDamage(int damage)
    {
        if(!isDamageable) return;

        else if(isDamageable && health > 0)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthBar();
            isDamageable = false;
            playerMovement.anim.SetTrigger("Damage");
            shieldObject.SetActive(true);
            ResetDamageable(1000).Forget();
        }
        else if(health <= 0)
        {
            isDamageable = false;
            playerMovement.anim.SetTrigger("Die");
            playerMovement.moveSpeed = 0f;
            hearthCounter -= 1;
            UpdateHearts(hearthCounter);
            if(hearthCounter <= 0) menuScript.OpenPanel(menuScript.gameOverPanel);  
            playerMovement.rb.isKinematic = true;
            Respawn().Forget();
        }
    }

    async UniTaskVoid ResetDamageable(int delayTime)
    {
        await UniTask.Delay(delayTime);
        isDamageable = true;
        shieldObject.SetActive(false);
    }

    async UniTaskVoid Respawn()
    {
        await UniTask.Delay(3000);
        shieldObject.SetActive(true);
        playerMovement.anim.SetTrigger("Respawn");
        health = 100;
        UpdateHealthBar();
        playerMovement.moveSpeed = 7f;
        playerTransform.position = spawnPoint.position;
        playerMovement.rb.isKinematic = false;
        await UniTask.Delay(2000);
        shieldObject.SetActive(false);
        isDamageable = true;
    }

    void UpdateHealthBar()
    {
        if (heathBar != null)
            heathBar.fillAmount = (float)health / maxHealth;
    }

    public void UpdateHearts(int heartCounts)
    {
        hearts[0].SetActive(false);
        hearts[1].SetActive(false);
        hearts[2].SetActive(false);
        for (int i = 0; i < heartCounts + 1; i++)
        {
            hearts[i].SetActive(true);
        }
    }
}
