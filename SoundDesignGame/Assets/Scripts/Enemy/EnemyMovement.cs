using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyMovement : MonoBehaviour
{
    private float speed;
    private float speedMultiplier;

    [SerializeField] private float difficulty;

    [SerializeField] private float changeStateCooldownTimer;

    [Header("MAX DISTANCE, TP ENEMY BACK")]
    [SerializeField] private float maxDistanceFromPlayer;

    [Header("RUNNING STATE")]
    [SerializeField] private float runningRange;
    [SerializeField] private float runningSpeed;

    [Header("WALKING STATE")]
    [SerializeField] private float walkingRange;
    [SerializeField] private float walkingSpeed;

    [Header("ATTACKING STATE")]


    [Header("FREEZE STATE")]
    [SerializeField] private float freezeTime;
    [SerializeField] private int freezeStatesRemaining;
    private bool isFrozen = false;

    private bool changeStateCooldown;

    private GameObject player;
    private GameManager gameManager;
    private PlayerController playerController;
    private EnemySounds enemySounds;

    public Enemy enemy;
    public enum Enemy
    {
        Walking,
        Running,
        Attacking,
        OutOfRange,
        Freeze,
        KilledPlayer,
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemySounds = GetComponentInChildren<EnemySounds>();
    }

    private void Update()
    {
        if (playerController.movement == PlayerController.Movement.InTutorial) return;
<<<<<<< Updated upstream

        speedMultiplier += Time.deltaTime * difficulty; //multiplier to increase difficulty over time
=======
        //speedMultiplier += Time.deltaTime * difficulty; //multiplier to increase difficulty over time
        if (enemy == Enemy.KilledPlayer) return;
>>>>>>> Stashed changes

        switch (enemy)
        {
            case Enemy.Walking:
                speed = walkingSpeed;
                enemySounds.EnemyFootstepsWalking();
                break;
            case Enemy.Running:
                speed = runningSpeed;
                enemySounds.EnemyFootstepsRunning();
                break;
            case Enemy.Attacking:
                speed = 0f;
                gameManager.PlayerDied();
                break;
            case Enemy.OutOfRange:
                speed = runningSpeed * 10f; //if player is way too far, make sure enemy stays inside a certain range so it doesnt get too far
                break;
            case Enemy.Freeze:
                speed = 0f;
                StartCoroutine("FreezeState");
                break;
            case Enemy.KilledPlayer:
                speed = 0f;
                break;
        }

        StateCheck();
    }

    private IEnumerator FreezeState()
    {
        freezeStatesRemaining -= 1;
        if (freezeStatesRemaining <= 0) yield break;
        isFrozen = true;
        yield return new WaitForSeconds(freezeTime);
        isFrozen = false;
    } 

    private IEnumerator Cooldown() //dont switch state too often
    {
        changeStateCooldown = true;
        yield return new WaitForSeconds(changeStateCooldownTimer);
        changeStateCooldown = false;
    }

    private void FixedUpdate()
    {
        if (playerController.movement == PlayerController.Movement.InTutorial || isFrozen) return;

        transform.position += new Vector3(0, 0, speed * speedMultiplier) * Time.deltaTime;
    }

    private void StateCheck()
    {
        if (isFrozen || changeStateCooldown) return; // dont change state if frozen or in change state cooldown

        if (Vector3.Distance(transform.position, player.transform.position) > maxDistanceFromPlayer)
            enemy = Enemy.OutOfRange;
        else if (Vector3.Distance(transform.position, player.transform.position) > runningRange)
            enemy = Enemy.Running;
        else if (Vector3.Distance(transform.position, player.transform.position) >= walkingRange)
            enemy = Enemy.Walking;
<<<<<<< Updated upstream
        else if (Vector3.Distance(transform.position, player.transform.position) < walkingRange)
            enemy = Enemy.Attacking;
=======
        else if (Vector3.Distance(transform.position, player.transform.position) < attackingRange)
        {
            enemy = Enemy.Attacking; //zodat we later meerdere hits kunnen implementen
        }
            
>>>>>>> Stashed changes
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, new Vector3(0f, transform.forward.z, 0f), maxDistanceFromPlayer * 2);

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, new Vector3(0f, transform.forward.z, 0f), runningRange * 2);

        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, new Vector3(0f, transform.forward.z, 0f), walkingRange * 2);
    }
} //yo