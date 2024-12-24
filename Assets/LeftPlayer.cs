using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPlayer : MonoBehaviour
{
    public Listener listener;
    public Transform cube;

    public float[] handpos1 = new float[3];
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        handpos1 = listener.LeftHandData;
        cube.position = new Vector3(10.0f * handpos1[0], -10.0f * handpos1[1], 0);
        cube.rotation = Quaternion.Euler(0, 0, handpos1[2]);
    }
}
