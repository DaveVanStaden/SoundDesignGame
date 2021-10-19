using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceWalking;
    public AudioSource audioSourceBreathing;
    [SerializeField] private AudioSource audioSourceDies;
    [SerializeField] private AudioSource audioSourceGun;

    private PlayerController player;
    private Exit exit;

    private bool isPlayingSound = false;
    private bool dead;
    private int i;
    private bool takingDmg;

    [Header("WALKING")]
    [SerializeField] private List<AudioClip> walkingFootsteps = new List<AudioClip>();
    [SerializeField] private float walkingTimeBetweenFootsteps;
    private int randomWalkFootstep;

    [Header("RUNNING")]
    [SerializeField] private List<AudioClip> runningFootsteps = new List<AudioClip>();
    [SerializeField] private float runningTimeBetweenFootsteps;
    private int randomRunFootstep;

    [Header("STAMINA")]
    [SerializeField] private List<AudioClip> breathing = new List<AudioClip>();

    [Header("DIE")]
    [SerializeField] private List<AudioClip> playerHitSound = new List<AudioClip>();
    [SerializeField] private List<AudioClip> enemyHitSound = new List<AudioClip>();
    [SerializeField] private AudioClip growl;
    [SerializeField] private AudioClip gunShot;
    [SerializeField] private AudioClip areYouDead;

    [Header("GUN")]
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip outOfBullets;
    [SerializeField] private AudioClip stunned;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        exit = GameObject.Find("End").GetComponent<Exit>();
    }

    public void WalkingSounds()
    {
        if (isPlayingSound) return;
        isPlayingSound = true;
        StartCoroutine("WalkingLoop");
    }

    public IEnumerator WalkingLoop()
    {
        if (exit.completedGame || player.movement == PlayerController.Movement.Tired || player.died) yield break;
        randomWalkFootstep = Random.Range(0, walkingFootsteps.Count);
        audioSourceWalking.PlayOneShot(walkingFootsteps[randomWalkFootstep]);
        yield return new WaitForSeconds(walkingTimeBetweenFootsteps);
        if (player.movement != PlayerController.Movement.Walking)
        {
            isPlayingSound = false;
            yield break;
        }
        StartCoroutine("WalkingLoop");
    }

    public void RunningSounds()
    {
        if (isPlayingSound) return;
        isPlayingSound = true;
        StartCoroutine("RunningLoop");
    }

    public IEnumerator RunningLoop()
    {
        if (exit.completedGame || player.movement == PlayerController.Movement.Tired || player.died) yield break;
        randomRunFootstep = Random.Range(0, runningFootsteps.Count);
        audioSourceWalking.PlayOneShot(runningFootsteps[randomRunFootstep]);
        if (player.staminaRemaining < player.maxStamina * 0.3f) runningTimeBetweenFootsteps = 0.65f;
        yield return new WaitForSeconds(runningTimeBetweenFootsteps);
        if (player.movement != PlayerController.Movement.Running)
        {
            isPlayingSound = false;
            yield break;
        }
        StartCoroutine("RunningLoop");
    }

    public void StaminaSound(float staminaVolume, int clipNumber)
    {
        if (player.died) return;
        if (player.movement == PlayerController.Movement.InTutorial) return;
        audioSourceBreathing.volume = staminaVolume;
        if (audioSourceBreathing.isPlaying) return;
        audioSourceBreathing.PlayOneShot(breathing[clipNumber]);
    }

    public void PlayerDyingSound()
    {
        if (player.died || takingDmg) return;
        StartCoroutine("PlayHitSound");
    }

    private IEnumerator PlayHitSound()
    {
        if (takingDmg) yield break;
        takingDmg = true;
        audioSourceDies.PlayOneShot(enemyHitSound[i]);
        audioSourceDies.PlayOneShot(growl);
        yield return new WaitForSeconds(1f);
        audioSourceDies.PlayOneShot(playerHitSound[i]);
        i++;
        takingDmg = false;
        yield break;
    }

    public void ShotSound()
    {
        audioSourceGun.PlayOneShot(shot);
        StartCoroutine("Stunned");
    }

    private IEnumerator Stunned()
    {
        yield return new WaitForSeconds(1.5f);
        audioSourceGun.clip = stunned;
        audioSourceGun.Play();
    }

    public void CantShotSound()
    {
        audioSourceGun.clip = outOfBullets;
        audioSourceGun.Play();
    }

    public IEnumerator DieSound()
    {
        yield return new WaitForSeconds(4f);
        audioSourceDies.PlayOneShot(areYouDead);
    }
}