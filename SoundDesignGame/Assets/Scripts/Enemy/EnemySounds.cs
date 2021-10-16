using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    //[SerializeField] private AudioSource audioSourceFeet;
    //[SerializeField] private AudioSource audioSourceMouth;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceRoar;
    private int randomFootstep;
    private bool isPlayingSound;

    [Header("ROAR")]
    [SerializeField] private AudioClip roar;

    [Header("FOOTSTEPS WALKING")]
    [SerializeField] private List<AudioClip> enemyFootsteps = new List<AudioClip>();
    [SerializeField] private float timeBetweenFootstepsWalking;

    [Header("FOOTSTEPS WALKING")]
    //[SerializeField] private List<AudioClip> enemyFootstepsRunning = new List<AudioClip>();
    [SerializeField] private float timeBetweenFootstepsRunning;
    private int randomWalkFootstep;
    [SerializeField] private float timeBetweenFootsteps;

    private EnemyMovement enemy;
    private PlayerController playerController;

    private void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<EnemyMovement>();
        RoarSounds();
        EnemyFootstepsWalking();
    }

    public void RoarSounds()
    {
        if (audioSourceRoar.isPlaying) return;
        audioSourceRoar.Play();
    }

    public void EnemyFootstepsWalking()
    {
        if (isPlayingSound) return;
        isPlayingSound = true;
        StartCoroutine("PlayFootstepsWalking");
    }

    private IEnumerator PlayFootstepsWalking()
    {
        randomFootstep = Random.Range(0, enemyFootsteps.Count);
        audioSource.PlayOneShot(enemyFootsteps[randomFootstep]);
        yield return new WaitForSeconds(timeBetweenFootstepsWalking);
        if (enemy.enemy != EnemyMovement.Enemy.Walking)
        {
            isPlayingSound = false;
            yield break;
        }
        StartCoroutine("PlayFootstepsWalking");
    }

    public void EnemyFootstepsRunning()
    {
        if (isPlayingSound) return;
        isPlayingSound = true;
        StartCoroutine("PlayFootstepsRunning");
    }

    private IEnumerator PlayFootstepsRunning()
    {
        randomFootstep = Random.Range(0, enemyFootsteps.Count);
        audioSource.PlayOneShot(enemyFootsteps[randomFootstep]);
        yield return new WaitForSeconds(timeBetweenFootstepsWalking);
        if (enemy.enemy != EnemyMovement.Enemy.Running)
        {
            isPlayingSound = false;
            yield break;
        }
        StartCoroutine("PlayFootstepsRunning");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine("ConstantRoar");
    }

    private IEnumerator EnemyFootsteps()
    {
        if (playerController.died) yield break;
        randomWalkFootstep = Random.Range(0, enemyFootsteps.Count);
        audioSource.PlayOneShot(enemyFootsteps[randomWalkFootstep]);
        yield return new WaitForSeconds(timeBetweenFootsteps);
        StartCoroutine("EnemyFootsteps");
    }
}