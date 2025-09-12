using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Zenject;

public class PlayerData : MonoBehaviour
{
    PlayerMovement playerMovement;
    bool isDamageable = true;
    
    public int health = 100;
    public int maxHealth = 100;
    public Transform spawnPoint;
    public Transform playerTransform;
    public GameObject shieldObject;
    public Image heathBar;


    [Inject]
    void ZenjectSetup(PlayerMovement _playerMovement)
    {
        playerMovement = _playerMovement;
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
}
