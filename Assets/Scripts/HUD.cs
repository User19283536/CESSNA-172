using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    public PlaneController controller;
    public FPScounter performanceData;
    public wheelController brakeController;
    public Text velocity;
    public Text altitude;
    public Text airPressure;
    public Text engine;
    public Text GForce;
    public Text Flaps;
    public Text brakes;
    public Text gWarning;
    public Text climbVelocity;
    public Text currentFps;
    public Text avgFps;
    public Text maxFPS;
    public Text minFPS;
    public Text simTime;
    public Text currentFrame;
    int displayMode = 1;
    int showDebugMenu = -1;

    float timer = 0f;
    float lastDangerousG;

    // Update is called once per frame
    void Update()
    {
        velocity.text = "Prêdkoœæ: " + (controller.localVelocity.z * 3.0 * 3.6f).ToString("F2") + "km/h";
        altitude.text = "Wysokoœæ: " + (240 + controller.planeBody.transform.position.y * 3.0f).ToString("F2") + "m";
        airPressure.text = "Gêstoœæ powietrza: " + controller.airDensity.ToString("F2") + " kg/m3";
        engine.text ="Obroty: " + (controller.engineRPM).ToString("F2") + "/minuta";

        if (Input.GetKeyDown(KeyCode.K))
        {
            showDebugMenu = -1 * showDebugMenu;

        }


        if (showDebugMenu == 1)
        {
            simTime.text = "Czas: " + Mathf.Round(performanceData.totalduration) + "s";
            currentFrame.text = "Klatka: " + Mathf.Round(performanceData.totalFrames);

            if (Input.GetKeyDown(KeyCode.L))
            {
                displayMode = -1 * displayMode;

            }

            if (displayMode == 1)
            {
                currentFps.text = "FPS " + Mathf.Round(performanceData.currentFps);
                avgFps.text = "Avg:" + Mathf.Round(performanceData.averageFps);
                maxFPS.text = "Max: " + Mathf.Round(performanceData.maxFPS);
                minFPS.text = "Min: " + Mathf.Round(performanceData.minFPS);
            }
            else if (displayMode == -1)
            {
                currentFps.text = "ms: " + Mathf.Round(1000 / performanceData.currentFps);
                avgFps.text = "Avg:" + Mathf.Round(1000 / performanceData.averageFps);
                maxFPS.text = "Min: " + Mathf.Round(1000 / performanceData.maxFPS);
                minFPS.text = "Max: " + Mathf.Round(1000 / performanceData.minFPS);
            }
        } else if (showDebugMenu == -1)
        {
            simTime.text = " ";
            currentFrame.text = " ";
            currentFps.text = " ";
            avgFps.text = " ";
            maxFPS.text = " ";
            minFPS.text = " ";
        }
        
        if (controller.flapsUsed==1)
        {
            Flaps.text = "Klapy: aktywne";
        }
        else
        {
            Flaps.text = "Klapy: niekatywne";
        }
        brakes.text = "Hamulce: " + brakeController.brakeLevel.ToString("F2") + "%";
        GForce.text = "Przeci¹¿enie: " + controller.localGForce.ToString("F2") + "g";
        climbVelocity.text = "Prêdkoœæ wznoszenia: " + (controller.planeBody.velocity.y * 3.0 * 3.6f).ToString("F2") + "km/h";

        if (controller.localGForce >= 3.8f)
        {
            timer = 75f;
            lastDangerousG = controller.localGForce;
        }

        if(timer<=0)
        {
            gWarning.text = " ";

        }
        else
        {
            gWarning.text = string.Format("Uwaga! Niebezpieczna wartoœæ przeci¹¿enia ({0:0.00}g)!", lastDangerousG);
        }
    }

    private void FixedUpdate()
    {

        if (timer > 0)
        {
            timer--;
        }

        if (timer < 0)
        {
            timer = 0;
        }

    }
}
