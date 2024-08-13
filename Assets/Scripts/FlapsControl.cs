using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapsControl : MonoBehaviour
{
    // Start is called before the first frame update
    int used = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && used == 0)
        {
            transform.Rotate(-45f, 0f, 0f);
            used = 1;

        }
        if (Input.GetKeyDown(KeyCode.P) && used == 1)
        {
            transform.Rotate(45f, 0f, 0f);
            used = 0;

        }
    }
}
