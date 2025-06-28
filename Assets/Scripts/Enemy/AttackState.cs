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
        _animator.SetTrigger("Attack");
    }
    
    public void Update()
    {
        if (Vector2.Distance(_enemyCtrl.transform.position, _enemyCtrl.player.position) > _enemyCtrl.attackRange)
        {
            _enemyCtrl.ChangeState(new ChaseState(_enemyCtrl));
            return;
        }
        
        PerformAttack();
    }
    
    public void Exit()
    {
        // Reset attack state if needed
        _animator.ResetTrigger("Attack");
    }
    
    protected abstract void PerformAttack();
}
