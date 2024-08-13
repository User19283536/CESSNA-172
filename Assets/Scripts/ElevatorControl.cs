using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Rotate(30f, 0f, 0f);

        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            transform.Rotate(-30f, 0f, 0f);

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Rotate(-30f, 0f, 0f);

        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            transform.Rotate(30f, 0f, 0f);

        }
    }
}
