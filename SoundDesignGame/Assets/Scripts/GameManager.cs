using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BackgroundNoise backgroundNoise;
    [SerializeField] private Tutorial tutorial;

    private void Start()
    {
        tutorial.StartCoroutine("StartGame");
        //backgroundNoise.StartCoroutine("PlayARandomNoise");
    }
}
