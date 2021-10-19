using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerSounds playerSounds;

    [SerializeField] private int health;

    [Header("STAMINA")]
    [SerializeField] private float minStamina;
    public float maxStamina;
    public float staminaRemaining;

    [Header("WALKING STATE")]
    [SerializeField] private float walkingSpeed;

    [Header("RUNNING STATE")]
    [SerializeField] private float runningSpeed;

    public Movement movement;
    public Stamina stamina;
    public EnemyMovement enemyMovement;

    public float speed;
    private float currentTime;
    private float maxTime = 7f;
    public bool died = false;
    private bool boost;
    private bool newSound;

    public bool canShoot = true;

    private Exit exit;

    public enum Movement
    {
        InTutorial,
        Walking,
        Running,
        Hit,
        Tired,
    }

    public enum Stamina
    {
        Full,
        High,
        Mid,
        Low,
        Out,
    }

    private void Start()
    {
        playerSounds = GetComponent<PlayerSounds>();
        enemyMovement.GetComponent<EnemyMovement>();
        movement = Movement.InTutorial;
        stamina = Stamina.Full;
        staminaRemaining = maxStamina;
        exit = GameObject.Find("End").GetComponent<Exit>();
    }

    private void Update()
    {
        if (exit.completedGame)
        {
            speed = 0f;
            return;
        }

        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
        }

        if (died) return;

        /*if (stamina == Stamina.Out)
        {
            // = Movement.Hit;
            speed = 0f;
            return;
        }*/

        switch (movement)
        {
            case Movement.InTutorial:
                speed = 0f;
                break;
            case Movement.Walking:
                speed = walkingSpeed;
                playerSounds.WalkingSounds();
                staminaRemaining += Time.deltaTime / 4f; //gain 1 stamina every 4 seconds (out of 10 max)
                break;
            case Movement.Running:
                speed = runningSpeed;
                playerSounds.RunningSounds();
                staminaRemaining -= Time.deltaTime / 1.5f; //lose 0.75 stamina every second
                break;
            case Movement.Hit:
                playerSounds.RunningSounds();
                if (currentTime <= 0f)
                {
                    if (health > 0)
                    {
                        health--;
                        speed = 8f;
                        playerSounds.PlayerDyingSound();
                        staminaRemaining = 5f;
                    }
                    if (health < 1) {
                        died = true;
                        speed = 0f;
                        playerSounds.StartCoroutine("DieSound");
                    }
                    currentTime = maxTime;
                }
                if (!boost) StartCoroutine("SpeedBoost");
                boost = true;
                break;
            case Movement.Tired:
                speed = 0f;
                Invoke("BackToWalking", 2f);
                break;
        }

        switch (stamina)
        {
            case Stamina.Full:
                playerSounds.StaminaSound(0.3f, 0);
                break;
            case Stamina.High:
                playerSounds.StaminaSound(0.4f, 0);
                break;
            case Stamina.Mid:
                playerSounds.StaminaSound(0.5f, 1);
                break;
            case Stamina.Low:
                playerSounds.StaminaSound(0.5f, 2);
                break;
            case Stamina.Out:
                playerSounds.StaminaSound(0.5f, 3);
                movement = Movement.Tired;
                break;
        }

        if (movement == Movement.InTutorial) return; //dont move or lose stamina while in tutorial

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ShootBullet();
        }

        Inputs();
        StaminaCheck();
    }

    private void BackToWalking()
    {
        staminaRemaining = 2f;
        movement = Movement.Walking;
    }

    private void Inputs()
    {
        if (movement == Movement.Hit || movement == Movement.Tired) return;

        if (Input.GetKey(KeyCode.Mouse0)) //running wanneer mouse0, else walking
            movement = Movement.Running;
        else
            movement = Movement.Walking;
    }

    private void StaminaCheck()
    {
        staminaRemaining = Mathf.Clamp(staminaRemaining, minStamina, maxStamina);

        if (staminaRemaining >= maxStamina)
            stamina = Stamina.Full;
        else if (staminaRemaining >= maxStamina * 0.75f)
            stamina = Stamina.High;
        else if (staminaRemaining >= maxStamina * 0.5f)
            stamina = Stamina.Mid;
        else if (staminaRemaining > minStamina)
            stamina = Stamina.Low;
        else if (staminaRemaining <= minStamina)
            stamina = Stamina.Out;
            
    }

    private void FixedUpdate() //movement
    {
        if (movement == Movement.InTutorial) return; //return als we nog in de tutorial zitten

        transform.position += new Vector3(0, 0, speed) * Time.deltaTime; //move the player
    }

    private IEnumerator SpeedBoost()
    {
        yield return new WaitForSeconds(6f);
        movement = Movement.Walking;
        boost = false;
    }

    private void ShootBullet()
    {
        if (!canShoot)
        {
            playerSounds.CantShotSound();
            
        }
        else
        {
            playerSounds.ShotSound();
            enemyMovement.StartCoroutine("GetShot");
            canShoot = false;
        }
    }
}