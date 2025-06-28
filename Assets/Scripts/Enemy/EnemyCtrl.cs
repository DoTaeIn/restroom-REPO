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
    public Transform player;
    public float chaseRange = 5f;
    public float coneAngle = 60f; // Total angle of the cone
    public int rayCount = 10;     // Number of rays in the cone
    public float range = 5f;
    public float attackRange;
    public float attackCooldown = 1f;
    public float stamina = 100f;
    public float grabStrength = 10f;
    public float concussionDuration = 2f;
    [SerializeField] Transform grabPoint;
    
    public EnemyAttackType attackType; 
    private Dictionary<EnemyAttackType, Func<AttackState>> attackStateFactory;
    
    
    
    IState currentState;
    public Animator animator; 
    public NavMeshAgent agent;
    public bool isAttacking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerCtrl>().transform;
        attackStateFactory = new Dictionary<EnemyAttackType, Func<AttackState>>()
        {
            { EnemyAttackType.Owner, () => new OwnerAttack(this) },
            //{ EnemyAttackType.Grab, () => new GrabAttackState(this) },
            //{ EnemyAttackType.Ranged, () => new RangedAttackState(this) }
        };
    }

    private void Start()
    {
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        player = FindFirstObjectByType<PlayerCtrl>().transform;
        currentState?.Update();

        if (caught)
        {
            player = grabPoint;
        }
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
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            caught = true;
            StartAttack();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            caught = false;
        }
    }
    
    private void StartAttack()
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
