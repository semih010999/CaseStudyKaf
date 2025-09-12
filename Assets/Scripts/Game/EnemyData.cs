using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

public class EnemyData : MonoBehaviour
{
    public int health = 100;
    public GameObject enemy;
    public EnemyMovement enemyMovement;
    public ParticleSystem bloodParticle;

    PlayerAttack playerAttack;
    GameManager gameManager;

    [Inject]
    void ZenjectSetup(PlayerAttack _playerAttack, GameManager _gameManager)
    {
        playerAttack = _playerAttack;
        gameManager = _gameManager;
        playerAttack.enemies.Add(this);
    }

    public void TakeDamage(int damage)
    {
        if(!enemyMovement.isLife) return;

        health -= damage;
        bloodParticle.Play();
        if(health <= 0 && enemyMovement.isLife)
        {
            enemyMovement.isLife = false;
            playerAttack.enemies.Remove(this);
            enemyMovement.agent.speed = 0f;
            enemyMovement.animator.SetTrigger("Die");
            CheckWaveCounter();
            DestroyObject(2000).Forget();
        }
    }

    async UniTaskVoid DestroyObject(int delayTime)
    {
        await UniTask.Delay(delayTime);
        Destroy(enemy);
    }

    void CheckWaveCounter()
    {
        gameManager.waveEnemyCounter--;
        gameManager.killCounter++;
        gameManager.UpdateKillCounterText();
        if(gameManager.waveEnemyCounter == 0)
        {
            gameManager.waveCounter++;
            gameManager.isStartWave = false;
            gameManager.StartNextWaveTimer(30);
        }
    }
}
