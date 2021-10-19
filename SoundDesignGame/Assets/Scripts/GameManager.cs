using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BackgroundNoise backgroundNoise;
    [SerializeField] private Tutorial tutorial;
    public bool playerIsDead = false;

    private GameObject enemy;
    private GameObject player;
    private PlayerController playerscript;

    private void Start()
    {
        tutorial = GameObject.Find("Player").GetComponentInChildren<Tutorial>();
        enemy = GameObject.Find("Enemy");
        player = GameObject.Find("Player");
        playerscript = GameObject.Find("Player").GetComponent<PlayerController>();
        tutorial.StartCoroutine("StartGame");
    }

    public void restartMode()
    {
        playerIsDead = true;
        //Todo: sound that plays saying something like "Press R to restart"
        

    }

    public void restart()
    {
        playerIsDead = false;
        playerscript.movement = PlayerController.Movement.InTutorial;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -485.0f);
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -540.8f);
        tutorial.StartCoroutine("StartGame");
    }

    private void Update()
    {
        if(playerIsDead && Input.GetKey(KeyCode.R))
        {
            restart();
        }
    }
}
