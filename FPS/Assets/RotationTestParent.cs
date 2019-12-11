using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTestParent : MonoBehaviour
{
    public RotationTest child;

    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float lastY;
    float nowY;

    void Update()
    {
        time += Time.deltaTime;
        lastY = nowY;
        nowY = transform.rotation.eulerAngles.y;

        if(time > 0.1 + Random.Range(0.0f, 0.2f))
        {
            child.targetYQueue.Enqueue(new System.Tuple<float, int>(nowY,(int)(nowY - lastY)));
            time = 0.0f;
        }
    }
}
