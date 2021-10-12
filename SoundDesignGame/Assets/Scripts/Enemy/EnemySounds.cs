using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] private AudioSource audioSourceFeet;
    [SerializeField] private AudioSource audioSourceMouth;
=======
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceRoar;
>>>>>>> Stashed changes
    private int randomFootstep;
    private bool isPlayingSound;

    [Header("ROAR")]
    [SerializeField] private AudioClip roar;

    [Header("FOOTSTEPS WALKING")]
    [SerializeField] private List<AudioClip> enemyFootsteps = new List<AudioClip>();
<<<<<<< Updated upstream
    [SerializeField] private float timeBetweenFootstepsWalking;

    [Header("FOOTSTEPS WALKING")]
    [SerializeField] private List<AudioClip> enemyFootstepsRunning = new List<AudioClip>();
    [SerializeField] private float timeBetweenFootstepsRunning;
=======
    private int randomWalkFootstep;
    [SerializeField] private float timeBetweenFootsteps;
>>>>>>> Stashed changes

    private EnemyMovement enemy;
    private PlayerController playerController;

    private void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<EnemyMovement>();
<<<<<<< Updated upstream
        RoarSounds();
    }

    public void RoarSounds()
    {
        if (audioSourceMouth.isPlaying) return;
        audioSourceMouth.PlayOneShot(roar);
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
        audioSourceFeet.PlayOneShot(enemyFootsteps[randomFootstep]);
        yield return new WaitForSeconds(timeBetweenFootstepsWalking);
        if(enemy.enemy != EnemyMovement.Enemy.Walking)
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
        audioSourceFeet.PlayOneShot(enemyFootsteps[randomFootstep]);
        yield return new WaitForSeconds(timeBetweenFootstepsWalking);
        if (enemy.enemy != EnemyMovement.Enemy.Running)
        {
            isPlayingSound = false;
            yield break;
        }
        StartCoroutine("PlayFootstepsRunning");
=======
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine("ConstantRoar");
        EnemyFootstepsWalking();
    }

    private IEnumerator ConstantRoar()
    {
        if (playerController.died) yield break;
        audioSourceRoar.PlayOneShot(constantRoar);
        yield return new WaitForSeconds(constantRoar.length);
        StartCoroutine("ConstantRoar");
    }

    public void EnemyFootstepsWalking()
    {
        StartCoroutine("EnemyFootsteps");
    }

    private IEnumerator EnemyFootsteps()
    {
        if (playerController.died) yield break;
        randomWalkFootstep = Random.Range(0, enemyFootsteps.Count);
        audioSource.PlayOneShot(enemyFootsteps[randomWalkFootstep]);
        yield return new WaitForSeconds(timeBetweenFootsteps);
        StartCoroutine("EnemyFootsteps");
>>>>>>> Stashed changes
    }
}