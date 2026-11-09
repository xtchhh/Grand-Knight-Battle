using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class Crusader : CrusaderFaction
{
    [Header("Movement")]
    private float moveSpeed;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float idleSpeed = 0f;
    private Vector3 directionToEnemy;
    private Vector3 directionToDestination;
    private Vector3 direction;

    [Header("AI")]
    private NavMeshAgent agent;
    private Vector3 destination;
    private bool canProceed;
    private float distanceToEnemy;

    [Header("Animation")]
    private Animator animator;

    [Header("State")]
    public PlayerState currentState;
    private float runBuffer;
    private float runBufferTime = 0.5f;

    [Header("Mouse Stuff")]
    private PlayerMouseBehavior mouseBehavior;

    [Header("Combat")]
    private bool canGenerateRandomAttack = true;

    public enum PlayerState
    {
        None,
        Idle,
        Walking,
        Running,
        Attacking,
        Dead
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.stoppingDistance = 4f;

        animator = GetComponent<Animator>();

        mouseBehavior = gameObject.AddComponent<PlayerMouseBehavior>();

        CacheEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        HandleStateBehavior();
        TransitionState();  
    }

    Vector3 DirectionToDestination(Vector3 target)
    {
        return (target - this.transform.position).normalized;
    }

    void HandleWalkState()
    {
        direction = DirectionToDestination(mouseBehavior.mouseWorldPos);
        agent.destination = mouseBehavior.mouseWorldPos;
        animator.Play("Walk");
        agent.speed = walkSpeed;
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    void HandleRunState()
    {
        direction = DirectionToDestination(mouseBehavior.mouseWorldPos);
        agent.destination = mouseBehavior.mouseWorldPos;
        animator.Play("Run");
        agent.speed = runSpeed;
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    void HandleIdleState()
    {
        direction = DirectionToDestination(this.transform.position); 
        destination = this.transform.position;
        agent.speed = idleSpeed;
        animator.Play("Idle");
        this.transform.rotation = Quaternion.identity;
    }

    void HandleAttackState()
    {
        float attackTimer = 0f;
        float randomAttackTime = 0f;
        if (canGenerateRandomAttack)
        {
            randomAttackTime = Random.Range(1, 4);
            canGenerateRandomAttack = false;
        }

        direction = DirectionToDestination(ClosestEnemy().transform.position);
        agent.destination = this.transform.position;
        agent.speed = idleSpeed;

        attackTimer += Time.deltaTime;
        //Debug.Log(randomAttackTime);

        if (attackTimer > randomAttackTime)
        {
            animator.SetTrigger("isAttacking");
            attackTimer = 0f;
            canGenerateRandomAttack = true;
        }
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    void HandleDeadState()
    {
        direction = DirectionToDestination(this.transform.position);
        animator.Play("Death");
        agent.destination = this.transform.position;
        agent.speed = idleSpeed;
        this.transform.rotation = Quaternion.identity;

        StartCoroutine(StartDeadStateJob());
    }

    IEnumerator StartDeadStateJob()
    {
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }

    void TransitionState()
    {
        if (agent.velocity == Vector3.zero)
        {
            currentState = PlayerState.Idle;
        }

        if (mouseBehavior.isHitValid)
        {
            currentState = PlayerState.Running;
        }

        if (health <= 0f)
            currentState = PlayerState.Dead;

        distanceToEnemy = Vector3.Distance(ClosestEnemy().transform.position, this.transform.position);

        if (distanceToEnemy <= 4f)
        {
            currentState = PlayerState.Attacking;
        }
    }

    void HandleStateBehavior()
    {
        switch (currentState)
        {
            case PlayerState.Walking:
                HandleWalkState();
                break;

            case PlayerState.Running:
                HandleRunState();
                break;

            case PlayerState.Dead:
                HandleDeadState();
                break;

            case PlayerState.Attacking:
                HandleAttackState();
                break;

            default:
                HandleIdleState();
                break;
        }
    }
}
