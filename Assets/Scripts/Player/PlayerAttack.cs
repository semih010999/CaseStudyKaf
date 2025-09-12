using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Zenject;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public List<EnemyData> enemies;

    PlayerMovement playerMovement;
    BulletObjectPool bulletObjectPool;
    int shootDelay = 500;
    int shootDelayAuto = 1000;
    bool canShoot = true;
    bool canShootAuto = true;

    [Header("Values")]
    public float bulletSpeed = 10f;
    public float attackRange = 10f;
    public GameObject[] foundEnemies;

    [Header("References")]
    public Transform autoShootFirePoint;
    public AudioSource shootAudio;
    public AudioSource autoShootAudio;
    
    [Header("Skills")]
    bool canSkill1 = true;
    public GameObject skill1Object;
    public LayerMask damageLayer;
    public Transform skill1Transform;
    public float skill1Range;
    public Image skill1Icon;

    [Inject]
    void ZenjectSetup(PlayerMovement _playerMovement, BulletObjectPool _bulletObjectPool)
    {
        playerMovement = _playerMovement;
        bulletObjectPool = _bulletObjectPool;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && canSkill1)
        {
            Skill1();
        }

        EnemyData target = GetClosestEnemyInRange();
        if (target != null)
        {
            Vector3 dir = (target.transform.position - autoShootFirePoint.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
            autoShootFirePoint.rotation = Quaternion.Slerp(autoShootFirePoint.rotation, lookRot, Time.deltaTime * 10f);

            if (canShootAuto)
            {
                AutoShoot();
            }
        }
    }

    void Shoot()
    {
        shootAudio.Play();
        GameObject bullet = bulletObjectPool.GetObject();
        bullet.transform.position = playerMovement.firePoint.position;
        bullet.transform.rotation = playerMovement.firePoint.rotation;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bullet.transform.forward * bulletSpeed;
        ResetBullet(bullet).Forget();

        playerMovement.anim.SetTrigger("Shoot");
        canShoot = false;
        ResetShoot().Forget();
    }

    void AutoShoot()
    {
        autoShootAudio.Play();
        GameObject bullet = bulletObjectPool.GetObjectAutoShoot();
        bullet.transform.position = autoShootFirePoint.position;
        bullet.transform.rotation = autoShootFirePoint.rotation;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bullet.transform.forward * bulletSpeed;
        ResetBulletAutoShoot(bullet).Forget();

        canShootAuto = false;
        ResetShootAuto().Forget();
    }

    void Skill1()
    {
        skill1Object.SetActive(true);
        skill1Icon.color =  new Color(1f, 1f, 1f, 0.5f);
        //shootAudio.Play();
        Collider[] hitObjects = Physics.OverlapSphere(skill1Transform.position, skill1Range, damageLayer);

        foreach (Collider obj in hitObjects)
        {
            EnemyData enemyData = obj.gameObject.GetComponent<EnemyData>();
            if (obj.CompareTag("Enemy") && enemyData.enemyMovement.isLife)
            {
                enemyData.TakeDamage(50);
                break;
            }
        }
        canSkill1 = false;
        ResetSkill1Activate(600).Forget();
        ResetSkill1(15000).Forget();
    }

    EnemyData GetClosestEnemyInRange()
    {
        EnemyData closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < attackRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    async UniTaskVoid ResetShoot()
    {
        await UniTask.Delay(shootDelay);
        canShoot = true;
    }

    async UniTaskVoid ResetShootAuto()
    {
        await UniTask.Delay(shootDelayAuto);
        canShootAuto = true;
    }

    async UniTaskVoid ResetBullet(GameObject bullet)
    {
        await UniTask.Delay(2000);
        bulletObjectPool.ReturnObject(bullet);
    }

    async UniTaskVoid ResetBulletAutoShoot(GameObject bullet)
    {
        await UniTask.Delay(2000);
        bulletObjectPool.ReturnObjectAutoShoot(bullet);
    }

    async UniTaskVoid ResetSkill1Activate(int value)
    {
        await UniTask.Delay(value);
        skill1Object.SetActive(false);
    }

    async UniTaskVoid ResetSkill1(int value)
    {
        await UniTask.Delay(value);
        skill1Icon.color =  new Color(1f, 1f, 1f, 1f);
        canSkill1 = true;
    }

    void OnDrawGizmosSelected()
{
    if (skill1Transform != null)
    {
        Gizmos.color = Color.red; // çizgi rengi
        Gizmos.DrawWireSphere(skill1Transform.position, skill1Range);
    }
}
}
