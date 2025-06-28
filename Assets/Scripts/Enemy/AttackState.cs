using UnityEngine;

public abstract class AttackState : IState
{
    EnemyCtrl _enemyCtrl;
    Animator _animator;
    
    public AttackState(EnemyCtrl enemyCtrl)
    {
        _enemyCtrl = enemyCtrl;
        _animator = enemyCtrl.animator;
    }
    
    public void Enter()
    {
        // Trigger attack animation
        if(_animator.GetParameter(0).name == "isMad")
            _animator.SetBool("isMad", true);
        _animator.SetBool("isAttacking", true);
        _enemyCtrl.agent.isStopped = true;
        _enemyCtrl.agent.ResetPath();
        _enemyCtrl.agent.velocity = Vector3.zero;
    }
    
    public void Update()
    {
        if (Vector2.Distance(_enemyCtrl.transform.position, _enemyCtrl.player.position) > _enemyCtrl.attackRange && !_enemyCtrl.isAttacking)
        {
            _enemyCtrl.ChangeState(new ChaseState(_enemyCtrl));
            return;
        }
        
        PerformAttack();
    }
    
    public void Exit()
    {
        // Reset attack state if needed
        _animator.SetBool("isAttacking", false);
        _enemyCtrl.agent.isStopped = false;
    }
    
    protected abstract void PerformAttack();
}
