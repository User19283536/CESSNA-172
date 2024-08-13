using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlaneController : MonoBehaviour
{

    [SerializeField] CinemachineFreeLook freeLookCam;
    [SerializeField] CinemachineVirtualCamera followCam;
    [SerializeField] CinemachineVirtualCamera fpsCam;

    public Transform massCenter;
    public Transform liftPoint;
    public Transform enginePoint;

    public Rigidbody planeBody;
    public float airDensity = 1.0f;
    public float wingArea;
    public float s;
    public float e = 1.0f;
    public float frontDragArea = 1.5f;
    public float sideDragArea = 5.5f;
    public int flapsUsed = 0;
    public float propRadius;

    public float liftVal;
    public float dragVal;

    public AnimationCurve Curve;
    public AnimationCurve frontDrag;
    public AnimationCurve thrustCurve;
    public AnimationCurve densityAtAltitude;
    public AnimationCurve RpmIncrease;

    public Vector3 velocity;
    public Vector3 localVelocity;

    public float angleofAttack = 4f;
    public float angleofAttackYaw;
    public float maxangleofAttack = 4f;
    public float maxangleofAttackYaw = 2f;
    public float engineRPM = 0f;

    public float aileronArea = 1f;
    public float elevatorArea = 2f;
    public float rudderArea=1f;

    public float aileronCoef = 1.0f;
    public float rudderCoef = 1.0f;
    public float elevatorCoef = 1.0f;

    public float flapFrontDragCoef = 1.35f;
    public float flapSideDragCoef = 1.05f;
    public float flapLiftCoef = 1.25f;

    public float localGForce;
    public Vector3 lastVelocity;

    [Header("User_Input")]
    public float inputPitch;
    public float inputRoll;
    public float inputYaw;

    void CalculateAOA()
    {
        angleofAttack = Vector3.Angle(planeBody.transform.forward, planeBody.velocity);
    }

    void TransformFrameOfReference()
    {
        velocity = planeBody.velocity;
        Quaternion planeRotation = Quaternion.Inverse(planeBody.rotation);
        localVelocity = planeRotation * velocity;
        CalculateAOA();
        CalculateG(Time.fixedDeltaTime);
    }

    void CalculateG(float dt)
    {

        float accelerationXInG = 3.0f *(planeBody.velocity.x - lastVelocity.x) / (dt * 9.81f);
        float accelerationZInG = 3.0f * (planeBody.velocity.z - lastVelocity.z) / (dt * 9.81f);
        float accelerationYInG = 3.0f * (planeBody.velocity.y - lastVelocity.y) / (dt * 9.81f);
        localGForce = Mathf.Sqrt(Mathf.Pow(accelerationXInG, 2) + Mathf.Pow(accelerationZInG, 2) + Mathf.Pow(accelerationYInG+1, 2));
        lastVelocity = planeBody.velocity;
    }

    void Start()
    {
        planeBody.centerOfMass = massCenter.localPosition;
    }

    void Update()
    {
        //get user input
        inputPitch = Input.GetAxis("Vertical");
        inputRoll = Input.GetAxis("Horizontal");
        inputYaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.T) && engineRPM < 2700)
        {
            engineRPM += RpmIncrease.Evaluate(engineRPM);

            if (engineRPM > 2700)
                engineRPM = 2700;
        }

        if (Input.GetKey(KeyCode.R) && engineRPM > 0)
        {
            engineRPM -= RpmIncrease.Evaluate(engineRPM);

            if (engineRPM < 0)
                engineRPM = 0;
        }

        if (Input.GetKeyDown(KeyCode.O) && flapsUsed == 0)
        {

            flapsUsed = 1;

        }
        if (Input.GetKeyDown(KeyCode.P) && flapsUsed == 1)
        {
            flapsUsed = 0;

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if(followCam.Priority == 0 && freeLookCam.Priority == 0 && fpsCam.Priority == 10)
            {
                freeLookCam.Priority = 10;
                followCam.Priority = 0;
                fpsCam.Priority = 0;
            } else if (followCam.Priority == 0 && freeLookCam.Priority == 10 && fpsCam.Priority == 0)
            {
                followCam.Priority = 10;
                freeLookCam.Priority = 0;
                fpsCam.Priority = 0;

            } else if(followCam.Priority == 10 && freeLookCam.Priority == 0 && fpsCam.Priority == 0)
            {
                fpsCam.Priority = 10;
                followCam.Priority = 0;
                freeLookCam.Priority = 0;

            }


        }
    }
    void FixedUpdate()
    {
        TransformFrameOfReference();

        //calculate air density at altitude
        airDensity = 1.0f*densityAtAltitude.Evaluate(planeBody.transform.position.y * 3.0f);

        //calculate thrust
        float thrust = (float)(0.5 * thrustCurve.Evaluate(engineRPM) * airDensity * 3.14159 * Mathf.Pow(propRadius,2));

        //apply thrust
        planeBody.AddForceAtPosition(enginePoint.forward * thrust, enginePoint.position);

        //calculating lift coef
        float LCoef = Curve.Evaluate(angleofAttack);

        //calculating lift
        float lift = 0.5f  * airDensity * Mathf.Pow(localVelocity.z,2) * LCoef  * wingArea;

        //calculating profile drag coeficient
        float DzCoef = frontDrag.Evaluate(Mathf.Abs(angleofAttack));

        //calculatng lift induced drag coeficient
        float DiCoef = (float)(Mathf.Pow(Curve.Evaluate(angleofAttack), 2)) / (3.14159f * e * (Mathf.Pow(s, 2) / wingArea));

        //calculating total drag
        float fDrag = 0.5f * airDensity * Mathf.Pow(localVelocity.z, 2) * (DzCoef + DiCoef) * frontDragArea; 
        float sDrag = 0.5f * airDensity * Mathf.Pow(localVelocity.x, 2) * (DzCoef + DiCoef) * sideDragArea;

        //alternative induced drag calculation (only source wikipedia thus dscarded)
        //float lfDrag = (float)(Mathf.Pow(lift, 2) / (0.5f * airDensity * Mathf.Pow(localVelocity.z, 2) * 3.14159 * Mathf.Pow(wingArea, 2)));

        //flaps modifiers
        if (flapsUsed == 1)
        {
            lift = lift * flapLiftCoef;
            fDrag = fDrag * flapFrontDragCoef;
            sDrag = sDrag * flapSideDragCoef;
        }


        liftVal = lift;
        dragVal = fDrag;

        //apply drag & lift
        planeBody.AddForceAtPosition(liftPoint.up * lift, liftPoint.position);
        planeBody.AddForce(planeBody.velocity * (-fDrag));
        planeBody.AddForce(planeBody.velocity * (-sDrag));

        //calculate control surface torque
        float aileronTorque = aileronCoef * airDensity * Mathf.Pow(Mathf.Sqrt(Mathf.Pow(localVelocity.z,2) + Mathf.Pow(localVelocity.x,2)), 2) * aileronArea;
        float elevatorTorque = elevatorCoef * airDensity * Mathf.Pow(Mathf.Sqrt(Mathf.Pow(localVelocity.z,2) + Mathf.Pow(localVelocity.y,2)), 2)  * elevatorArea;
        float rudderTorque = rudderCoef * airDensity * Mathf.Pow(Mathf.Sqrt(Mathf.Pow(localVelocity.z, 2) + Mathf.Pow(localVelocity.x, 2)), 2) * rudderArea;

        //add control surface torque
        planeBody.AddTorque(transform.up * rudderTorque * inputYaw);
        planeBody.AddTorque(transform.forward * aileronTorque * inputRoll);
        planeBody.AddTorque(transform.right * elevatorTorque * inputPitch);

    }
}
