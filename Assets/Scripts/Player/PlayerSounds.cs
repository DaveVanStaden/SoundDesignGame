using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceWalking;
    [SerializeField] private AudioSource audioSourceBreathing;
    [SerializeField] private AudioSource audioSourceDies;

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
        if (exit.completedGame) yield break;
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
        if (exit.completedGame) yield break;
        randomRunFootstep = Random.Range(0, runningFootsteps.Count);
        audioSourceWalking.PlayOneShot(runningFootsteps[randomRunFootstep]);
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
        if (dead) return;
        if (player.movement == PlayerController.Movement.InTutorial) return;
        audioSourceBreathing.volume = staminaVolume;
        if (audioSourceBreathing.isPlaying) return;
        audioSourceBreathing.PlayOneShot(breathing[clipNumber]);
    }

    public void PlayerDyingSound()
    {
        if (dead || takingDmg) return;
        i++;
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
        takingDmg = false;
        yield break;
    }

    private void ShotSound()
    {
        audioSourceDies.PlayOneShot(gunShot);
    }
}