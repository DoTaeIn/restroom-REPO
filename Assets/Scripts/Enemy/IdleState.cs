using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    EnemyCtrl _enemyCtrl;
    Animator _anim;
    NavMeshAgent _agent;
    private Transform playerCtrl;
    
    private float _coneAngle = 60f; // Total angle of the cone
    private int _rayCount = 10;     // Number of rays in the cone
    private float _range = 5f;
    float idleDuration;
    float idleTimer;
    
    
    
    public IdleState(EnemyCtrl enemyCtrl)
    {
        _enemyCtrl = enemyCtrl;
        _anim = enemyCtrl.animator;
        _coneAngle = enemyCtrl.coneAngle;
        playerCtrl = enemyCtrl.player;
        _rayCount = enemyCtrl.rayCount;
        _range = enemyCtrl.range;
        _agent = enemyCtrl.agent;
        idleDuration = Random.Range(1f, 3f);
        idleTimer = 0f;
    }
    
    public void Enter()
    {
        //Entering idle state
    }

    public void Update()
    {
        DetectArea();

        if (_enemyCtrl.playerDetected)
        {
            _enemyCtrl.ChangeState(new ChaseState(_enemyCtrl));
            return;
        }
        
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            _enemyCtrl.ChangeState(new WanderState(_enemyCtrl));
        }
    }

    public void Exit()
    {
        
    }
    
    void DetectArea()
    {
        float coneAngle = 60f;
        int rayCount = 10;
        float range = 5f;
        float halfCone = coneAngle / 2f;
        Vector2 forward = _enemyCtrl.transform.right;
            
        _enemyCtrl.playerDetected = false;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -halfCone + (coneAngle / (rayCount - 1)) * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(
                forward.x * Mathf.Cos(rad) - forward.y * Mathf.Sin(rad),
                forward.x * Mathf.Sin(rad) + forward.y * Mathf.Cos(rad)
            );

            RaycastHit2D hit = Physics2D.Raycast(_enemyCtrl.transform.position, dir, range);
            Debug.DrawRay(_enemyCtrl.transform.position, dir * range, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _enemyCtrl.playerDetected = true;
                break;
            }
        }
    }
}
