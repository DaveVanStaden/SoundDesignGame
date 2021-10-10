using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceWalking;
    [SerializeField] private AudioSource audioSourceBreathing;

    private PlayerController player;

    private bool isPlayingSound = false;

    [Header("SNEAKING")]
    [SerializeField] private List<AudioClip> sneakingFootsteps = new List<AudioClip>();
    [SerializeField] private float sneakingTimeBetweenFootsteps;
    private int randomSneakFootstep;

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

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void TutorialSounds()
    {

    }

    public void SneakingSounds()
    {
        
    }

    public void WalkingSounds()
    {
        if (isPlayingSound) return;
        isPlayingSound = true;
        StartCoroutine("WalkingLoop");
    }

    public IEnumerator WalkingLoop()
    {
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
        if (player.movement == PlayerController.Movement.InTutorial) return;
        audioSourceBreathing.volume = staminaVolume;
        if (audioSourceBreathing.isPlaying) return;
        audioSourceBreathing.PlayOneShot(breathing[clipNumber]);
    }
}