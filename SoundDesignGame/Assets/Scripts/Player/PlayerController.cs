using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerSounds playerSounds;
    private BackgroundNoise bgNoise;

    [Header("STAMINA")]
    [SerializeField] private float minStamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRemaining;

    [Header("TUTORIAL STATE")]

    [Header("SNEAKING STATE")]
    [SerializeField] private float sneakingSpeed; //we doen voor nu alleen nog walking/running

    [Header("WALKING STATE")]
    [SerializeField] private float walkingSpeed;

    [Header("RUNNING STATE")]
    [SerializeField] private float runningSpeed;

    public Movement movement;
    public Stamina stamina;

    private float speed;
    public bool died = false;

    public enum Movement
    {
        InTutorial,
        Sneaking,
        Walking,
        Running,
        StandStill,
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
        movement = Movement.InTutorial;
        stamina = Stamina.Full;
        staminaRemaining = maxStamina;
    }

    private void Update()
    {
        if (died) return;

        if (stamina == Stamina.Out)
        {
            movement = Movement.StandStill;
            speed = 0f;
            return;
        }

        switch (movement)
        {
            case Movement.InTutorial:
                speed = 0f;
                playerSounds.TutorialSounds();
                break;
            case Movement.Sneaking:
                speed = sneakingSpeed;
                playerSounds.SneakingSounds(); //not implemented
                break;
            case Movement.Walking:
                speed = walkingSpeed;
                playerSounds.WalkingSounds();
                staminaRemaining += Time.deltaTime / 4f; //gain 1 stamina every 4 seconds (out of 10 max)
                break;
            case Movement.Running:
                speed = runningSpeed;
                playerSounds.RunningSounds();
                staminaRemaining -= Time.deltaTime; //lose 1 stamina every second
                break;
            case Movement.StandStill:
                died = true;
                break;
        }

        switch (stamina)
        {
            case Stamina.Full:
                playerSounds.StaminaSound(0.1f, 0);
                break;
            case Stamina.High:
                playerSounds.StaminaSound(0.2f, 0); //same sound but louder
                break;
            case Stamina.Mid:
                playerSounds.StaminaSound(0.3f, 1);
                break;
            case Stamina.Low:
                playerSounds.StaminaSound(0.4f, 2);
                break;
            case Stamina.Out:
                playerSounds.StaminaSound(0.5f, 3);
                break;
        }

        if (movement == Movement.InTutorial) return; //dont move or lose stamina while in tutorial

        Inputs();
        StaminaCheck();
    }

    private void Inputs()
    {
        if (Input.GetKey(KeyCode.UpArrow)) //running wanneer keycode arrowup, else walking
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
}