using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, -3) * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(0, 0, -1) * Time.deltaTime;
        }
    }
}
