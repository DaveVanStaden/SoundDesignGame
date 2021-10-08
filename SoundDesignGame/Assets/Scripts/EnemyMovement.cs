using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, -2) * Time.deltaTime;
    }
}
