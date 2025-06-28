using UnityEngine;

public class DeadState : IState
{
    EnemyCtrl _enemyCtrl;
    private Animator _anim;

    public DeadState(EnemyCtrl enemyCtrl)
    {
        _enemyCtrl = enemyCtrl;
        _anim = enemyCtrl.animator;
    }
    
    public void Enter()
    {
        //animator trigger dead
    }
    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
