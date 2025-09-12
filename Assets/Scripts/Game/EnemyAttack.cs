using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyAttack : MonoBehaviour
{
    [Header("Values")]
    public float attackRange = 1.5f;
    public int attackDamage = 10;       
    int attackRate = 1000;       
    public bool canAttack = true;

    [Header("References")]
    public LayerMask damageLayer; 
    public Transform firePoint;
    public EnemyMovement enemyMovement;

    float nextAttackTime = 0f;

    void Update()
    {
        // Eğer saldırı cooldown’u dolduysa
        if (canAttack)
        {
            // Çember içinde player var mı?
            Collider[] hitObjects = Physics.OverlapSphere(firePoint.position, attackRange, damageLayer);

            foreach (Collider obj in hitObjects)
            {
                if (obj.CompareTag("Player") && enemyMovement.isLife)
                {
                    canAttack = false;
                    Attack(obj);
                    enemyMovement.agent.speed = 0f;
                    enemyMovement.ResetSpeed().Forget();
                    break;
                }

                else if (obj.CompareTag("Tower") && enemyMovement.isLife)
                {
                    canAttack = false;
                    AttackObject(obj);
                    enemyMovement.ResetSpeed().Forget();
                    break;
                }
            }
        }
    }

    void Attack(Collider colObject)
    {
        enemyMovement.animator.SetTrigger("Attack");
        PlayerData health = colObject.GetComponent<PlayerData>();
        health.TakeDamage(attackDamage);
        ResetAttack().Forget();
    }

    void AttackObject(Collider colObject)
    {
        enemyMovement.animator.SetTrigger("Attack");
        ObjectData health = colObject.GetComponent<ObjectData>();
        health.TakeDamage(attackDamage);
        ResetAttack().Forget();
    }

    async UniTaskVoid ResetAttack()
    {
        await UniTask.Delay(attackRate);
        canAttack = true;
    }

    // Scene’de çemberi görmek için
    void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }
}