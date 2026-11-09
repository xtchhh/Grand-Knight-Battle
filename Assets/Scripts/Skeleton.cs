using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class Skeleton : SkeletonTeam
{
    [Header("Movement")]
    private float moveSpeed;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float idleSpeed = 0f;
    private Vector3 direction;

    [Header("AI")]
    private NavMeshAgent agent;
    private bool canGenerateNumber;

    [Header("Animation")]
    private Animator animator;
    private float distanceToEnemy;

    [Header("State")]
    public AIState currentState;
    private Navigation navPosition;
    AIState[] stateCollection = {AIState.Walking, AIState.Running, AIState.Idle};
    private bool canTransition = true;

    [Header("Transitions")]
    public float transitionTimer;
    public float transitionTime;
    public int randomStateIndex;

    [Header("Combat")]
    private bool canGenerateRandomAttack = true;

    public enum AIState
    {
        None,
        Idle,
        Walking,
        Running,
        Attacking,
        Dead
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //adding navagent is expensive maybe?
    void Start()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;

        animator = GetComponent<Animator>();
        navPosition = gameObject.AddComponent<Navigation>();

        transitionTimer = 13f;
        canTransition = true;

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

    #region State Behaviours

    void HandleWalkState()
    {
        direction = DirectionToDestination(agent.destination);
        animator.Play("Skeleton_Rig|Walk");
        agent.speed = walkSpeed;
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    void HandleRunState()
    {
        direction = DirectionToDestination(agent.destination);
        animator.Play("Skeleton_Rig|Run");
        agent.speed = runSpeed;
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    //or stop the agent instead of setting destination to its transform position
    void HandleIdleState()
    {
        direction = DirectionToDestination(this.transform.position);
        agent.destination = this.transform.position;
        agent.speed = idleSpeed;
        animator.Play("Skeleton_Rig|Idle");
        this.transform.rotation = Quaternion.LookRotation(direction);
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
        animator.Play("Skeleton_Rig|Death");
        agent.destination = this.transform.position;
        agent.speed = idleSpeed;
        moveSpeed = idleSpeed;
        this.transform.rotation = Quaternion.identity;

        StartCoroutine(StartDeadStateJob());
    }

    IEnumerator StartDeadStateJob()
    {
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }

    #endregion

    #region State Transitions
    void HandleMainStateTransition()
    {
        if (canGenerateNumber)
        {
            transitionTime = Random.Range(5, 20);
            canGenerateNumber = false;
        }

        transitionTimer += Time.deltaTime;

        if (transitionTimer > transitionTime)
        {
            randomStateIndex = Random.Range(0, stateCollection.Length);
            agent.destination = navPosition.GetDestination();
            currentState = stateCollection[randomStateIndex];
            transitionTimer = 0f;
            canGenerateNumber = true;
        }
    }

    void HandleAttackStateTransition()
    {
        distanceToEnemy = Vector3.Distance(ClosestEnemy().transform.position, this.transform.position);

        if (distanceToEnemy <= 5f)
            currentState = AIState.Attacking;
    }

    void HandleDeadStateTransition()
    {
        if (health <= 0f)
        {
            currentState = AIState.Dead;
        }
    }
    #endregion

    #region State Handlers
    void TransitionState()
    {
        HandleAttackStateTransition();
        HandleDeadStateTransition();
        HandleMainStateTransition();
    }

    void HandleStateBehavior()
    {
        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState();
                break;

            case AIState.Walking:
                HandleWalkState();
                break;

            case AIState.Running:
                HandleRunState();
                break;

            case AIState.Attacking:
                HandleAttackState();
                break;

            case AIState.Dead:
                HandleDeadState();
                break;
        }
    }
    #endregion
}
