using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    [HideInInspector]
    public ObjectPool parent = null;

    public void Push()
    {
        parent.Push(this);
    }
}