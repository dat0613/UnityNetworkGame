using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class RotationTest : MonoBehaviour
{
    float lastY = 0.0f;
    public float targetY = 0.0f;
    public float lerp = 1.0f;

    float nowPacketTime = 0.0f;
    float lastPacketTime = 0.0f;

    public Queue<Tuple<float, int>> targetYQueue = new Queue<Tuple<float, int>>();

    // public float yPerFrame = 0.0f;
    float yPer1second;

    void Update()
    {
        while(targetYQueue.Count > 0)
        {
            lastPacketTime = nowPacketTime;
            nowPacketTime = Time.time;

            float timediffer = nowPacketTime - lastPacketTime;

            var tuple = targetYQueue.Dequeue();

            lastY = targetY;
            targetY = tuple.Item1;

            var direction = tuple.Item2;
            if(direction == 0)
            {
                yPer1second = 0.0f;
            }
            else
            {
                yPer1second = (targetY - lastY) / timediffer;
            }

        }

        var euler = transform.eulerAngles;
        euler.y += yPer1second * Time.deltaTime;

        transform.rotation = Quaternion.Euler(euler);

        // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, targetY, 0.0f), 20.0f);
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0.0f, targetY, 0.0f), Time.deltaTime * lerp);
    }
}
