using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BackgroundNoise backgroundNoise;
    [SerializeField] private Tutorial tutorial;

    private void Start()
    {
        tutorial = GameObject.Find("Player").GetComponentInChildren<Tutorial>();
        tutorial.StartCoroutine("StartGame");
    }
}
