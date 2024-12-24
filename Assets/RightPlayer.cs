using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPlayer : MonoBehaviour
{
    public Listener listener;
    public Transform cube;

    public float[] handpos2 = new float[3];
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        handpos2 = listener.RightHandData;
        cube.position = new Vector3(10.0f * handpos2[0], -10.0f * handpos2[1], 0);
        cube.rotation=Quaternion.Euler(0, 0, handpos2[2]);
    }
}
