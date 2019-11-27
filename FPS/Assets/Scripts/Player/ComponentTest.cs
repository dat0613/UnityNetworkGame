using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class ComponentTest : MonoBehaviourMinNet
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            RPC("SendTest", MinNetRpcTarget.AllViaServer, 123456, 123.456f, "와 센즈!!");
        }
    }

    public void SendTest(int num, float num2, string sanz)
    {
        Debug.Log(num);
        Debug.Log(num2);
        Debug.Log(sanz);
    }

    public void RecvTest(string test)
    {
        Debug.Log(test);
    }
}
