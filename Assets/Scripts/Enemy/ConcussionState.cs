using UnityEngine;

public class ConcussionState : IState
{
    EnemyCtrl _enemyCtrl;
    float _concussionDuration = 2f; // Duration of the concussion effect
    float _concussionTimer = 0f; // Timer to track concussion duration
    
    public ConcussionState(EnemyCtrl enemyCtrl)
    {
        _enemyCtrl = enemyCtrl;
        _concussionDuration = enemyCtrl.concussionDuration;
    }
    
    public void Enter()
    {
        // Logic for entering the concussion state
        Debug.Log("Entering Concussion State");
    }

    public void Update()
    {
        _concussionTimer += Time.deltaTime;
        
        if (_concussionTimer >= _concussionDuration)
        {
            _enemyCtrl.ChangeState(new IdleState(_enemyCtrl));
            return;
        }
    }

    public void Exit()
    {
        
    }
}
