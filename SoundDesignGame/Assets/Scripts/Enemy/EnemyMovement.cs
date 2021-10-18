using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyMovement : MonoBehaviour
{
    private float speed;

    [SerializeField] private float changeStateCooldownTimer;

    [Header("MAX DISTANCE, TP ENEMY BACK")]
    [SerializeField] private float maxDistanceFromPlayer;

    [Header("RUNNING STATE")]
    [SerializeField] private float runningSpeed;

    [Header("WALKING STATE")]
    [SerializeField] private float walkingRange;
    [SerializeField] private float walkingSpeed;

    [Header("ATTACKING STATE")]
    [SerializeField] private float attackingRange;

    [Header("FREEZE STATE")]
    [SerializeField] private float freezeTime;
    [SerializeField] private int freezeStatesRemaining;
    private bool isFrozen = false;

    [Header("Gets Shot")]
    [SerializeField] private float GetShotTimer;

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

        //speedMultiplier += Time.deltaTime * difficulty; //multiplier to increase difficulty over time

        switch (enemy)
        {
            case Enemy.Walking:
                speed = walkingSpeed;
                //enemySounds.EnemyFootstepsWalking();
                break;
            case Enemy.Running:
                speed = runningSpeed;
                //enemySounds.EnemyFootstepsWalking(); //we hebben nog geen geluid voor rennend monster, niet super balangrijk
                break;
            case Enemy.Attacking:
                speed = 0f;
                break;
            case Enemy.OutOfRange:
                speed = runningSpeed * 10f; //if player is way too far, make sure enemy stays inside a certain range so it doesnt get too far
                break;
            case Enemy.Freeze:
                speed = 0f;
                StartCoroutine("FreezeState");
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
    public IEnumerator GetShot()
    {
        isFrozen = true;
        yield return new WaitForSeconds(GetShotTimer);
        isFrozen = false;
        
    }

    private void FixedUpdate()
    {
        if (playerController.movement == PlayerController.Movement.InTutorial || isFrozen) return;

        transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
    }

    private void StateCheck()
    {
        if (isFrozen || changeStateCooldown) return; // dont change state if frozen or in change state cooldown

        if (Vector3.Distance(transform.position, player.transform.position) > maxDistanceFromPlayer)
            enemy = Enemy.OutOfRange;
        else if (Vector3.Distance(transform.position, player.transform.position) > walkingRange)
            enemy = Enemy.Running;
        else if (Vector3.Distance(transform.position, player.transform.position) >= attackingRange)
            enemy = Enemy.Walking;
        else if (Vector3.Distance(transform.position, player.transform.position) < attackingRange)
            enemy = Enemy.Attacking;
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, new Vector3(0f, transform.forward.z, 0f), maxDistanceFromPlayer * 2);

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, new Vector3(0f, transform.forward.z, 0f), walkingRange * 2);

        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, new Vector3(0f, transform.forward.z, 0f), attackingRange * 2);
    }
} //yo