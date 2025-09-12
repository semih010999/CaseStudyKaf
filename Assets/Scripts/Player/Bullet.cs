using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 10;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyData enemyData = col.gameObject.GetComponent<EnemyData>();
            if(enemyData == null) return;
            if(enemyData.enemyMovement.isLife)
            {
                enemyData.TakeDamage(bulletDamage);
                gameObject.SetActive(false);
            }
        }
    }
}
