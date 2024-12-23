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
        UnityEngine.Debug.Log("Right Hand Data in Player: " + string.Join(", ", handpos1));
        cube.position = new Vector3(10.0f * handpos1[0], -10.0f * handpos1[1], 0);
        cube.rotation = Quaternion.Euler(0, 0, handpos1[2]); // rotation around z axis
    }
}
