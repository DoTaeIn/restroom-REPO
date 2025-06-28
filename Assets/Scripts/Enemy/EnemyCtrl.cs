using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyAttackType
{
    Owner,
    Son,
    Dog,
    Jammin
}

public class EnemyCtrl : MonoBehaviour
{
    public bool playerDetected = false;
    public bool caught = false;
    public PlayerCtrl player;
    public float chaseRange = 5f;
    public float coneAngle = 60f; // Total angle of the cone
    public int rayCount = 10;     // Number of rays in the cone
    public float range = 5f;
    public float detectRange = 5f;
    public float attackRange = 3f;
    public float attackCooldown = 1f;
    public float stamina = 100f;
    public float grabStrength = 10f;
    public float concussionDuration = 2f;
    public Transform grabPoint;
    
    public EnemyAttackType attackType; 
    private Dictionary<EnemyAttackType, Func<AttackState>> attackStateFactory;
    
    
    
    public IState currentState;
    public Animator animator; 
    public NavMeshAgent agent;
    SpriteRenderer spriteRenderer;
    Rigidbody rb;
    public bool isAttacking = false;
    [SerializeField] private GameObject lazer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerCtrl>();
        attackStateFactory = new Dictionary<EnemyAttackType, Func<AttackState>>()
        {
            { EnemyAttackType.Owner, () => new OwnerAttack(this) },
            { EnemyAttackType.Son, () => new SonAttack(this, lazer) },
            //{ EnemyAttackType.Ranged, () => new RangedAttackState(this) }
        };
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        //agent.enabled = false;
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        player = FindFirstObjectByType<PlayerCtrl>();
        currentState?.Update();

        if (caught)
        {
            player.transform.position = grabPoint.position;
        }

        bool temp = agent.velocity.magnitude > 0.01f;
        spriteRenderer.flipX = agent.velocity.x < 0;
        animator.SetBool("isWalk",  temp );
        
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
    public void DecreaseGrab(float amount)
    {
        grabStrength -= amount;
        if (grabStrength < 0)
        {
            grabStrength = 0;
            ChangeState(new ConcussionState(this));
        }
    }
    
    
    public void StartAttack()
    {
        if (attackStateFactory.TryGetValue(attackType, out var createState))
        {
            ChangeState(createState());
        }
        else
        {
            Debug.LogWarning($"No attack state defined for {attackType}");
        }
    }
}
