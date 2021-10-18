using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();

    private Transform player;
    [SerializeField] private float distToPlayer;
    private float startingDist;
    private float currentDist;
    public bool completedGame;
    private bool almostThere;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        startingDist = Vector3.Distance(transform.position, player.position);
    }

    private void Update()
    {
        currentDist = Vector3.Distance(transform.position, player.position);
        

        if(currentDist < startingDist * 0.25f && !almostThere)
        {
            almostThere = true;
            GettingClose();
        }

        if(currentDist < 2f && !completedGame)
        {
            completedGame = true;
            StartCoroutine("TheEnd");
        }
    }

    private void GettingClose()
    {
        audioSource.PlayOneShot(sounds[0]); //i think i can hear the city
    }

    private IEnumerator TheEnd()
    {
        audioSource.PlayOneShot(sounds[1]); //poort
        yield return new WaitForSeconds(sounds[1].length + 0.5f);
        audioSource.PlayOneShot(sounds[2]); //thank god you made it
        yield return new WaitForSeconds(sounds[2].length + 2f);
        audioSource.PlayOneShot(sounds[3]); //helicopter
        yield break;
    }
}