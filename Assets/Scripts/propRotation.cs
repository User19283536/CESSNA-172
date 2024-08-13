using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotVelocity = 0f;
    public float currentRPM = 0f;
    public Rigidbody proppeler;
    public PlaneController controller;


    public Vector3 angularVel;
    void Start()
    {

        //overriding max angular velocity limit
        proppeler.maxAngularVelocity = 24;
    }

    // Update is called once per frame
    void Update()
    {
        currentRPM = controller.engineRPM;


        proppeler.AddTorque(transform.forward * currentRPM/5);

        angularVel = proppeler.angularVelocity;

    }
}
