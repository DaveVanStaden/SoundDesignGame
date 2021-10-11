using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceFeet;
    [SerializeField] private AudioSource audioSourceMouth;
    private int randomFootstep;
    private bool isPlayingSound;

    [Header("ROAR")]
    [SerializeField] private AudioClip roar;

    [Header("FOOTSTEPS WALKING")]
    [SerializeField] private List<AudioClip> enemyFootsteps = new List<AudioClip>();
    [SerializeField] private float timeBetweenFootstepsWalking;

    [Header("FOOTSTEPS WALKING")]
    [SerializeField] private List<AudioClip> enemyFootstepsRunning = new List<AudioClip>();
    [SerializeField] private float timeBetweenFootstepsRunning;

    private EnemyMovement enemy;

    private void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<EnemyMovement>();
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
    }
}