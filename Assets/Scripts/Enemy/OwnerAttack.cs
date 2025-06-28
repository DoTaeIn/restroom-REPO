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

    public override void Exit()
    {
        // restore movement flag
        var playerCtrl = enemy.player.GetComponent<PlayerCtrl>();
        playerCtrl.caught = false;

        // detach
        enemy.player.transform.SetParent(null);

        // *** NEW: clear physics momentum ***
        Rigidbody2D rb = enemy.player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        base.Exit();               
    }

    public override void Enter()
    {
        base.Enter();                // stops agent, triggers Attack animation

        // disable player movement
        enemy.player.caught = true;    

        // attach them to your grab point
        enemy.player.transform.SetParent(enemy.grabPoint.transform);
        enemy.player.transform.localPosition = Vector3.zero;
        enemy.stamina = 100f;
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
            if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) <= enemy.attackRange)
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
