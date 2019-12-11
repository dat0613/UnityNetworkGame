using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SoundManager.Instance.PlaySound("Shot", Vector3.zero, 100, 1);
        }
    }
}
