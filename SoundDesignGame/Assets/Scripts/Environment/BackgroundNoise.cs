using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNoise : MonoBehaviour
{
    private AudioSource audioSource;
    private int randomSound;

    [Header("LIST OF RANDOM NOISES")]
    [SerializeField] private List<AudioClip> randomNoises = new List<AudioClip>();

    [Header("RANDOM WAIT TIME BETWEEN 2 NOISES")]
    [SerializeField] private int minWaitTime;
    [SerializeField] private int maxWaitTime;
    private int randomWaitTime;

    [Header("CONSTANT NOISE")]
    [SerializeField] private AudioClip wind;
    [SerializeField] private AudioClip crickets;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator PlayARandomNoise() //speelt een random sound uit de list
    {
        randomSound = Random.Range(0, randomNoises.Count); //kiest een random geluid
        randomWaitTime = Random.Range(minWaitTime, maxWaitTime); //kiest een random wachttijd tussen 2 geluiden

        audioSource.PlayOneShot(randomNoises[randomSound]);
        yield return new WaitForSeconds(randomNoises[randomSound].length + randomWaitTime); //wacht de lengte van de audioclip af + extra random waittime
        StartCoroutine("PlayARandomNoise");
    }

    public IEnumerator ConstantNoise()
    {
        yield break;
    }
}