using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BackgroundNoise bgNoise;
    private Tutorial tutorial;
    private PlayerController playerController;
    private EnemyMovement enemy;
    private PlayerSounds playerSounds;

    private void Start()
    {
        bgNoise = GameObject.Find("BackGroundNoises").GetComponent<BackgroundNoise>();
        tutorial = GameObject.Find("Player").GetComponentInChildren<Tutorial>();
        playerSounds = GameObject.Find("Player").GetComponentInChildren<PlayerSounds>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        enemy = GameObject.Find("Enemy").GetComponent<EnemyMovement>();

        tutorial.StartCoroutine("StartGame");
        bgNoise.Wind();
    }

    private void Update()
    {
        if (playerController.movement == PlayerController.Movement.InTutorial) return;
    }

    public void PlayerDied()
    {
        playerController.movement = PlayerController.Movement.StandStill;
        enemy.enemy = EnemyMovement.Enemy.KilledPlayer;
        playerSounds.PlayerDyingSound();
    }
}
