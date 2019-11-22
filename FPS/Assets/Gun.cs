using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Vector3 add;
    public Transform leftHand;

    public float overHeating;
    public float shotDelay;
    public Transform muzzleTransform;

    private float delta = 0.0f;
    public ParticleSystem muzzleFlashParticle;

    public Trail trailPrefab;

    [SerializeField]
    MeshRenderer meshRenderer;
 
    public void SetVisible(bool visible)
    {
        meshRenderer.enabled = visible;
    }


    void Start()
    {

    }

    void Update()
    {
        delta += Time.deltaTime;
    }

    // 판정은 카메라에서 하고 이 함수에서는 눈속임용 총알을 그려주는 역할만 함
    // 그렇기 때문에 판정이 끝난 오브젝트를 인자로 받음
    public bool Shot(GameObject hitObject)
    {
        if(delta > shotDelay)
        {   
            delta = 0.0f;

            if(hitObject != null)
            {// 맞은 객체가 있다면
            
            }

            return true;
        }
        return false;
    }

    public void CreateTrail(Vector3 endPoint, Vector3 muzzlePosition, bool local)
    {
        var trail = Instantiate(trailPrefab, muzzlePosition, Quaternion.identity);
        var dif = endPoint - muzzlePosition;

        if (muzzleFlashParticle.isPlaying)
        {
            muzzleFlashParticle.Stop();
            muzzleFlashParticle.Clear();
        }

        muzzleFlashParticle.Play();

        trail.SetOption(endPoint, 300, local);

        SoundManager.Instance.PlaySound("Shot", muzzlePosition);
    }

    public void SetTransform(Transform tr)
    {
        transform.position = tr.position;// + tr.rotation * GripPosition.position;// - GripPosition.localPosition;
        //transform.LookAt(leftHand); //Quaternion.Euler(tr.rotation.eulerAngles + add);
        //transform.rotation = Quaternion.Euler(tr.rotation.eulerAngles + add);
        
        var gap = leftHand.position - tr.position;

        transform.rotation = Quaternion.LookRotation(gap);
    }
}
