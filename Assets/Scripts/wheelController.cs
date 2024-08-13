using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelController : MonoBehaviour
{
    public float dynFriction = 5;
    public float statFriction = 5;
    public float brakeLevel = 0;
    public Collider coll;

    // Start is called before the first frame update

    float counter = 0;

    void Start()
    {
        coll = GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(coll.material.dynamicFriction < 5.1f)
            {
                coll.material.dynamicFriction += 0.5f;
                coll.material.staticFriction += 0.5f;
                counter++;
            }

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (coll.material.dynamicFriction > 0.1f)
            {
                coll.material.dynamicFriction -= 0.5f;
                coll.material.staticFriction -= 0.5f;
                counter--;
            }


        }

        brakeLevel = (counter / 10f) * 100f;
        dynFriction = coll.material.dynamicFriction;

    }
}
