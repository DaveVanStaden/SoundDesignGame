using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private int randomFootstep;
    private bool isPlayingSound;

    [Header("ROAR")]
    [SerializeField] private AudioClip singleRoar;
    [SerializeField] private AudioClip constantRoar;

    [Header("FOOTSTEPS WALKING")]
    [SerializeField] private AudioClip enemyFootsteps;

    private EnemyMovement enemy;

    private void Start()
    {
        enemy = GameObject.Find("Enemy").GetComponent<EnemyMovement>();
        ConstantRoarSounds();
        EnemyFootstepsWalking();


    }

    private void Update()
    {
        ConstantRoarSounds();
    }

    public void ConstantRoarSounds()
    {
        if (audioSource.isPlaying) return;
        audioSource.PlayOneShot(constantRoar);
    }

    public void SingeRoar()
    {
        audioSource.PlayOneShot(singleRoar);
    }

    public void EnemyFootstepsWalking()
    {
        audioSource.PlayOneShot(enemyFootsteps);
    }
}