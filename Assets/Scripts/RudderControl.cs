using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RudderControl : MonoBehaviour
{
    // Start is called before the first frame update
    public PlaneController controller;
    public float rotation = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0f, 30f, 0f);
 
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            transform.Rotate(0f, -30f, 0f);

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0f, -30f, 0f);

        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            transform.Rotate(0f, 30f, 0f);

        }





    }
}
