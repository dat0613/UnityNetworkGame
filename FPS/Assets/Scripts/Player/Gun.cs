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

    public float maxOverheat = 30.0f;
    public float nowOverheat = 0.0f;
    public float lerp = 1.0f;
    public float maxCoolingTime = 4.0f;

    public float heatPerShot = 2.0f;

    private float coolingPoint = 0.0f;
    public float coolingPerSecond = 1.0f;

    private bool canShot = true;

    Coroutine coolingSystem = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    public void SetVisible(bool visible)
    {
        meshRenderer.enabled = visible;
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    public Material GetMaterial()
    {
        return meshRenderer.material;
    }

    public void OverheatReset()
    {
        nowOverheat = 0.0f;
    }

    void LateUpdate()
    {
        delta += Time.deltaTime;
    }

    IEnumerator gunCoolingSystem()
    {
        while(true)
        {
            if (nowOverheat > maxOverheat)
            {// 총기가 최대치 까지 과열됨
                canShot = false;
                
                nowOverheat = maxOverheat;
                SoundManager.Instance.PlaySound("OverHeat");
                yield return new WaitForSeconds(maxCoolingTime);
                nowOverheat = maxOverheat - 1.0f;
            }
            else
            {
                canShot = true;

                coolingPoint += Time.deltaTime * coolingPerSecond;
                nowOverheat -= coolingPoint * Time.deltaTime;
                
                if(nowOverheat <= 0.0f)
                {
                    nowOverheat = 0.0f;
                    coolingPoint = 0.0f;
                }

                yield return 0;
            }
        }
    }

    void OnEnable()
    {
        if (coolingSystem == null)
        {
            coolingSystem = StartCoroutine("gunCoolingSystem");
        }
    }

    void OnDisable()
    {
        if(coolingSystem != null)
        {
            StopCoroutine(coolingSystem);
            coolingSystem = null;
        }
    }

    // 판정은 카메라에서 하고 이 함수에서는 눈속임용 총알을 그려주는 역할만 함
    // 그렇기 때문에 판정이 끝난 오브젝트를 인자로 받음
    public bool Shot(GameObject hitObject)
    {
        if(delta > shotDelay && canShot)
        {   
            delta = 0.0f;

            nowOverheat += heatPerShot;
            coolingPoint = 0.0f;

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

        SoundManager.Instance.PlaySound("Shot", muzzlePosition, 45.0f, 0.6f);
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
