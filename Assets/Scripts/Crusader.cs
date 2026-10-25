using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Crusader : CrusaderTeam
{
    [Header("Combat")]
    private float attackTimer;

    [Header("Movement")]
    private float moveSpeed;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float idleSpeed = 0f;
    //private float retreatSpeed = 2f;
    private Vector3 directionToEnemy;

    [Header("Agent")]
    private NavMeshAgent enemyAgent;

    [Header("Animation")]
    private Animator animator;

    [Header("State")]
    private PlayerState currentState;

    private enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Dead
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAgent = gameObject.AddComponent<NavMeshAgent>();
        enemyAgent.autoBraking = false;
        enemyAgent.updateRotation = false;

        animator = GetComponent<Animator>();

        CacheEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        SelfDestruct();
    }

    void Rotation()
    {
        directionToEnemy = (ClosestEnemy().transform.position - this.transform.position).normalized; // why do i have to subtract from target instead of opposite to create direction?
        this.transform.rotation = Quaternion.LookRotation(directionToEnemy);
    }

    void Attack()
    {
        attackTimer += Time.deltaTime;
        float randomAttackTime = Random.Range(1, 4);
        float updatedDistanceToEnemy = Vector3.Distance(this.transform.position, ClosestEnemy().transform.position);

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
        currentState = PlayerState.Walking;
        //play walking anim
        //set destination to mouse click position
        moveSpeed = walkSpeed;
        //rotation
    }

    void HandleRunState()
    {
        currentState = PlayerState.Running;
        //play running anim
        //set destination to mouse click position
        moveSpeed = runSpeed;
        //rotation
    }

    void HandleIdleState()
    {
        currentState = PlayerState.Idle;
        //play idle anim
        //set destination to current pos/ or stop it?
        moveSpeed = idleSpeed;
        //rotation
    }

    void HandleDeadState()
    {
        currentState = PlayerState.Dead;
        //play dead anim
        //set destination to current pos/ or stop it?
        moveSpeed = idleSpeed;
        //rotation
    }

    void HandleCurrentState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:

                if (this.transform.position == Vector3.zero)
                {
                    HandleIdleState();
                }
                break;

            case PlayerState.Walking:

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    HandleWalkState();
                }
                break;

            case PlayerState.Running:
                
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    HandleRunState();
                }
                break;

            case PlayerState.Dead:

                if (health <= 0f)
                {
                    HandleDeadState();
                }
                break;
        }
    }
}
