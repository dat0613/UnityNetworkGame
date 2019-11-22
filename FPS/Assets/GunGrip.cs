using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGrip : MonoBehaviour
{
    public Transform gunPosition;
    public Gun gun;
    Transform handTransform;

    void Start()
    {
        handTransform = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);
    }

    void Update()
    {
    }

    void GunPositionUpdate() 
    {
        gun.SetTransform(handTransform);
    }

    void LateUpdate()
    {
        if(gun != null)
            GunPositionUpdate();
    }

}