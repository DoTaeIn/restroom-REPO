using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    EnemyCtrl _enemyCtrl;
    private Animator _anim;
    private NavMeshAgent _agent;
    Transform _playerCtrl;
    
    public ChaseState(EnemyCtrl enemyCtrl)
    {
        _enemyCtrl = enemyCtrl;
        _anim = enemyCtrl.animator;
        _playerCtrl = enemyCtrl.player;
        _agent = enemyCtrl.agent;
    }
    
    public void Enter()
    {
        //Entering chase state
        _anim.SetBool("isMad", true);
    }

    public void Update()
    {
        float dist = Vector2.Distance(_enemyCtrl.transform.position, _playerCtrl.position);
        //Debug.Log(dist);
        if (dist > _enemyCtrl.detectRange)
        {
            // still too far—keep chasing
            _enemyCtrl.ChangeState(new WanderState(_enemyCtrl));
        }
        else if (dist <= _enemyCtrl.detectRange && dist > _enemyCtrl.attackRange)
        {
            _agent.SetDestination(_playerCtrl.position);
        }
        else if (dist <= _enemyCtrl.attackRange)
        {
            // now you're in range—go attack!
            Debug.Log("ChaseState: Player in range, switching to attack state.");
            _enemyCtrl.StartAttack();
        }

    }

    public void Exit()
    {
        //Exiting chase state
    }
}
