using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Crusader : CrusaderTeam
{
    [Header("Movement")]
    private float moveSpeed;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float idleSpeed = 0f;
    private Vector3 directionToEnemy;
    private Vector3 directionToDestination;

    [Header("AI")]
    private NavMeshAgent agent;
    private Vector3 destination;

    [Header("Animation")]
    private Animator animator;

    [Header("State")]
    public PlayerState currentState;

    [Header("Test Positions")]
    public Transform pos1;
    public Transform pos2;

    public enum PlayerState
    {
        None,
        Idle,
        Walking,
        Running,
        Dead
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;

        animator = GetComponent<Animator>();

        CacheEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        SelfDestruct();
        HandleStateBehavior();
        Move();
        TransitionState();
    }

    void Move()
    {
        directionToDestination = (destination - this.transform.position).normalized;
        
        agent.destination = destination;
        agent.speed = moveSpeed;
    }

    void Attack()
    {
        float attackTimer = 0f;
        float randomAttackTime = Random.Range(1, 4);
        float updatedDistanceToEnemy = Vector3.Distance(this.transform.position, ClosestEnemy().transform.position);

        attackTimer += Time.deltaTime;
        directionToEnemy = (ClosestEnemy().transform.position - this.transform.position).normalized;

        if (updatedDistanceToEnemy <= 4f)
        {
            if (attackTimer > randomAttackTime)
            {
                animator.SetTrigger("isAttacking");
                attackTimer = 0f;
            }
        }
    }

    void HandleWalkState()
    {
        destination = pos1.position;
        animator.Play("Walk");
        moveSpeed = walkSpeed;
        this.transform.rotation = Quaternion.LookRotation(directionToDestination);
    }

    void HandleRunState()
    {
        destination = pos2.position;
        animator.Play("Run");
        moveSpeed = runSpeed;
        this.transform.rotation = Quaternion.LookRotation(directionToDestination);
    }

    void HandleIdleState()
    {
        destination = this.transform.position;
        moveSpeed = idleSpeed;
        animator.Play("Idle");
        this.transform.rotation = Quaternion.identity;
    }

    void HandleDeadState()
    {
        currentState = PlayerState.Dead;
        //play dead anim
        //set destination to current pos/ or stop it?
        moveSpeed = idleSpeed;
        //this.transform.rotation = Quaternion.identity;
    }

    void TransitionState()
    {
        if (agent.velocity == Vector3.zero)
            currentState = PlayerState.Idle;

        if (Mouse.current.leftButton.isPressed)
            currentState = PlayerState.Walking;

        if (Mouse.current.rightButton.isPressed)
            currentState = PlayerState.Running;
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

            case PlayerState.Idle:
                HandleIdleState();
                break;
        }
    }
}
