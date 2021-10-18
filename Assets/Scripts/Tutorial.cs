using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private float timeBetweenVoiceLines;

    [Header("SOUNDS PRE TUTORIAL")]
    [SerializeField] private List<AudioClip> walkingFootsteps = new List<AudioClip>();
    [SerializeField] private AudioClip alarm;
    [SerializeField] private AudioClip whatsThatNoise;
    [SerializeField] private AudioClip needToGetOut;
    [SerializeField] private float timeBetweenFootsteps;
    [SerializeField] private float amountOfFootsteps;
    private int randomFootstep;

    [Header("SOUNDS FOR TUTORIAL")]
    [SerializeField] private AudioClip tutorialStory;
    [SerializeField] private AudioClip tutorialControls;

    public AudioSource audioSource;
    private PlayerController player;
    private GameManager gameManager;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public IEnumerator StartGame()
    {
        StartCoroutine("Footsteps");
        yield return new WaitForSeconds(amountOfFootsteps * timeBetweenFootsteps);
        audioSource.PlayOneShot(alarm);
        yield return new WaitForSeconds(5f);
        StartCoroutine("PlayTutorial");
        yield break;
    }

    public IEnumerator Footsteps()
    {
        for (int i = 0; i < amountOfFootsteps + 4; i++) //+4 is hoeveel stappen hij zet terwijl alarm afgaat 
        {
            randomFootstep = Random.Range(0, walkingFootsteps.Count);
            audioSource.PlayOneShot(walkingFootsteps[randomFootstep]);
            yield return new WaitForSeconds(timeBetweenFootsteps);

            if (i == amountOfFootsteps)
            {
                yield return new WaitForSeconds(3f);
                audioSource.PlayOneShot(whatsThatNoise);
            }

        }
    }

    public IEnumerator PlayTutorial()
    {
        yield return new WaitForSeconds(2f);
        audioSource.PlayOneShot(tutorialStory);
        yield return new WaitForSeconds(tutorialStory.length + 0.5f); //wacht tot de story is afgelopen



        audioSource.PlayOneShot(tutorialControls);
        yield return new WaitForSeconds(tutorialControls.length + timeBetweenVoiceLines); //wacht tot de controls zijn uitgelegd
        player.movement = PlayerController.Movement.Walking;
        yield return new WaitForSeconds(3f);
        audioSource.volume = 0.3f;
        audioSource.PlayOneShot(needToGetOut);
        yield return new WaitForSeconds(needToGetOut.length + 0.5f);
        yield break;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            player.movement = PlayerController.Movement.Walking;
            player.stamina = PlayerController.Stamina.Full;
        }
    }
}