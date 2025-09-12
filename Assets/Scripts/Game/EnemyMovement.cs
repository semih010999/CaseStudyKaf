using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using Zenject;
using Cysharp.Threading.Tasks;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypointsEnemy;
    int currentWaypointIndex = 0;
    public float currentSpeed;

    [Header("References")]
    public NavMeshAgent agent;
    public Animator animator;
    public GameManager gameManager;
    public bool isLife = true;

    [Inject]
    void ZenjectSetup(GameManager _gameManager)
    {
        gameManager = _gameManager;
    }

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        waypointsEnemy = gameManager.waypoints;

        if (waypointsEnemy.Count > 0)
            agent.SetDestination(waypointsEnemy[0].position);

        currentSpeed = agent.speed;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (currentWaypointIndex == 2) return;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1);
            agent.SetDestination(waypointsEnemy[currentWaypointIndex].position);
        }

        animator?.SetBool("isRun", true);
    }

    public async UniTaskVoid ResetSpeed()
    {
        await UniTask.Delay(1000);
        agent.speed = currentSpeed;
    }
}
