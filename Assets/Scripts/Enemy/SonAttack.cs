using UnityEngine;

public class SonAttack : AttackState
{
    EnemyCtrl _enemyCtrl;
    Animator animator;
    GameObject attackPrefab;
    
    
    
    float attackTimer = 0f;
    
    [Tooltip("Degrees per second")]
    public float rotationSpeed = 90f;
    [Tooltip("Seconds to wait before starting to rotate")]
    public float delayBeforeRotation = 1f;
    [Tooltip("Total attack duration, including delay")]
    public float totalAttackTime = 3f;
    
    public SonAttack(EnemyCtrl enemyCtrl, GameObject attackPrefab) : base(enemyCtrl)
    {
        _enemyCtrl = enemyCtrl;
        animator = enemyCtrl.animator;
        this.attackPrefab = attackPrefab;
    }
    
    
    protected override void PerformAttack()
    {
        // 1) On first frame, initialize the attack
        if (!_enemyCtrl.isAttacking)
        {
            _enemyCtrl.isAttacking = true;
            attackTimer = 0f;                          // reset our timer
            animator.SetTrigger("Attack");
            _enemyCtrl.agent.ResetPath();
            attackPrefab.GetComponent<Laser>().isAttacking = true;
        }

        // 2) Tick the timer
        attackTimer += Time.deltaTime;

        // 3) Only start rotating once we've waited long enough
        if (attackTimer >= delayBeforeRotation)
        {
            // rotate around Z by rotationSpeed degrees/sec
            attackPrefab.transform.Rotate(
                0f, 
                0f, 
                rotationSpeed * Time.deltaTime, 
                Space.Self
            );
        }

        // 4) When the overall attack time elapses, clean up and change state
        if (attackTimer >= totalAttackTime)
        {
            attackPrefab.GetComponent<Laser>().isAttacking = false;
            _enemyCtrl.isAttacking = false;
            _enemyCtrl.ChangeState(new ConcussionState(_enemyCtrl));
        }
    }

    void DrawRay()
    {
        
    }
}
