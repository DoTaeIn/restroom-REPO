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
        _anim.SetBool("isChasing", true);
    }

    public void Update()
    {
        if (Vector2.Distance(_enemyCtrl.transform.position, _playerCtrl.position) <= 3f)
        {
            _agent.SetDestination(_playerCtrl.position);
        }
    }

    public void Exit()
    {
        //Exiting chase state
    }
}
