using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceRoar;
    private int randomFootstep;
    private bool isPlayingSound;

    [Header("ROAR")]
    [SerializeField] private AudioClip roar;
    [SerializeField] private AudioClip enemyHit;

    [Header("ENEMY NOISES")]
    [SerializeField] private List<AudioClip> enemyFootsteps = new List<AudioClip>();
    [SerializeField] private float timeBetweenWalkingFootsteps;
    [SerializeField] private float timeBetweenRunningFootsteps;

    private float timeBetweenFootsteps;

    private EnemyMovement enemy;
    private PlayerController playerController;

    private void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<EnemyMovement>();
        //RoarSounds();
        StartCoroutine("PlayFootsteps");
    }

    /*(public void RoarSounds()
    {
        if (audioSourceRoar.isPlaying) return;
        audioSourceRoar.Play();
    }*/

    private IEnumerator PlayFootsteps()
    {
        if (enemy.enemy == EnemyMovement.Enemy.KilledPlayer || enemy.enemy == EnemyMovement.Enemy.Freeze) yield break;
        TimeBetweenFootsteps();
        int randomFootstep = Random.Range(0, enemyFootsteps.Count);
        audioSource.PlayOneShot(enemyFootsteps[randomFootstep]);
        yield return new WaitForSeconds(timeBetweenFootsteps);
        StartCoroutine("PlayFootsteps");
    }

    private void TimeBetweenFootsteps()
    {
        if (enemy.enemy == EnemyMovement.Enemy.Walking) timeBetweenFootsteps = timeBetweenWalkingFootsteps;
        else
            timeBetweenFootsteps = timeBetweenRunningFootsteps;
    }

    private IEnumerator HitSound()
    {
        audioSourceRoar.PlayOneShot(enemyHit);
        yield return new WaitForSeconds(enemy.shotTime + 0.1f);
        StartCoroutine("PlayFootsteps");
    }
}