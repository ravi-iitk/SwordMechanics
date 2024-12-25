using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPlayer : MonoBehaviour
{
    public Listener listener;
    public Transform cube;

    public float[] handpos1 = new float[3];//changed number to 3

    void Update()
    {

        handpos1 = listener.RightHandData;
        // UnityEngine.Debug.Log("Right Hand Data in Player: " + string.Join(", ", handpos1));
        if(handpos1[0] == 0 && handpos1[1] == 0 && handpos1[2] == 0){
            return;
        }
        cube.localPosition = new Vector3(10.0f * (handpos1[0]-0.5f), -10.0f * (handpos1[1]-0.5f), 0); //changed to localPosition
        cube.rotation = Quaternion.Euler(0, 0, 90+handpos1[2]); // rotation around z axis
    }
}
