using UnityEngine;
using UnityEngine.AI;

public class WanderState : IState
{
    EnemyCtrl _enemyCtrl;
    NavMeshAgent agent;
    
    private float patrolRadius = 10f;
    private float minDistance = 1f;
    

    public WanderState(EnemyCtrl enemy)
    {
        _enemyCtrl = enemy;
        agent = enemy.agent;
    }
    
    public void Enter()
    {
        // Code to execute when entering the wander state
        Debug.Log("Entering Wander State");
        SetNewDestination();
    }

    public void Update()
    {
        DetectArea();
        
        
        if (_enemyCtrl.playerDetected)
        {
            _enemyCtrl.ChangeState(new ChaseState(_enemyCtrl));
            return;
        }
        
        if (!agent.pathPending && agent.remainingDistance < minDistance)
        {
            _enemyCtrl.ChangeState(new IdleState(_enemyCtrl));
            //SetNewDestination();
        }
        
    }

    public void Exit()
    {
        //Exiting wander state
    }
    
    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += _enemyCtrl.transform.position;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    void DetectArea()
    {
        float coneAngle = 60f;
        int rayCount = 10;
        float range = 5f;
        float halfCone = coneAngle / 2f;
        Vector3 lookDir = agent.velocity.normalized;
            
        _enemyCtrl.playerDetected = false;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -halfCone + (coneAngle / (rayCount - 1)) * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(
                lookDir.x * Mathf.Cos(rad) - lookDir.y * Mathf.Sin(rad),
                lookDir.x * Mathf.Sin(rad) + lookDir.y * Mathf.Cos(rad)
            );

            RaycastHit2D hit = Physics2D.Raycast(_enemyCtrl.transform.position, dir, range, LayerMask.GetMask("Player"));
            Debug.DrawRay(_enemyCtrl.transform.position, dir * range, Color.red);

            if (hit.collider != null )
            {
                if(hit.collider.CompareTag("Player"))
                {
                    _enemyCtrl.playerDetected = true;
                    break;
                }
                Debug.Log(hit.collider.name);
            }
        }
    }
}
