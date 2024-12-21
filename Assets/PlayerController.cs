using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContrller : MonoBehaviour
{
    public Listener listener;
    public Transform cube;

    public float[] handpos = new float[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        handpos = listener.handData;
        cube.position = new Vector3(10.0f*handpos[0], -10.0f*handpos[1], 0);
    }
}
