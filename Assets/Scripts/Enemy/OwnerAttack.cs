using UnityEngine;

public class OwnerAttack : AttackState
{
    private EnemyCtrl enemy;
    private float attackTimer = 0f;
    private bool hasDealtDamage = false;

    public OwnerAttack(EnemyCtrl enemyCtrl) : base(enemyCtrl)
    {
        //coolTime = enemyCtrl.attackCooldown;
        enemy = enemyCtrl;
    }
    
    protected override void PerformAttack()
    {
        if (!enemy.isAttacking)
        {
            enemy.isAttacking = true;
            enemy.animator.SetTrigger("GrabAttack");
            attackTimer = 0f;
            hasDealtDamage = false;
            enemy.agent.ResetPath(); // Stop moving while attacking
        }

        attackTimer += Time.deltaTime;

        // Example: Deal damage after 0.5 seconds (sync with animation grab moment)
        if (!hasDealtDamage && attackTimer >= 0.5f)
        {
            if (Vector2.Distance(enemy.transform.position, enemy.player.position) <= enemy.attackRange)
            {
                Debug.Log("Enemy grabs and damages player!");
                enemy.player.GetComponent<PlayerCtrl>()?.TakeDamage(10); // Assuming PlayerCtrl has TakeDamage()
                hasDealtDamage = true;
            }
        }

        // End attack after attack cooldown
        if (attackTimer >= enemy.attackCooldown)
        {
            attackTimer = 0;
            //enemy.isAttacking = false;
            //enemy.ChangeState(new ChaseState(enemy));
        }
    }
}
