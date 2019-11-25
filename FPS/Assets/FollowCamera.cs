using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class FollowCamera : MonoBehaviour
{
    public GameObject targetObject = null;

    public float distance = 6.0f;
    public float zoomDistance = 1.0f;
    public float lerp = 10.0f;
    private float nowDistance;
    private float targetDistance = 0.0f;

    private float targetZoomCenterY = 2.0f;

    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;
    public Vector3 vector = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector2 center;
    public float height;
    public bool viewMode = false;

    public Camera cam = null;

    public bool mouseLock = true;

    public GameObject lookObject = null;

    public void SetMouseLock(bool mouseLock)
    {
        this.mouseLock = mouseLock;

        if(mouseLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            lookObject = null;
        }
        else
            Cursor.lockState = CursorLockMode.None;

        Cursor.visible = !mouseLock;
    }

    static FollowCamera instance = null;
    public static FollowCamera Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360)
            angle += 360;
        if(angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    void Start()
    {
        SetMouseLock(true);
        var angles = transform.eulerAngles;

        vector.x = angles.y;
        vector.y = angles.x;

        targetDistance = nowDistance = distance;
    }

    public void SetZoom(bool zoomMode)
    {
        if(zoomMode)
        {
            targetDistance = zoomDistance;
            targetZoomCenterY = 1.0f;
        }
        else
        {
            targetDistance = distance;
            targetZoomCenterY = 2.0f;
        }
    }

    public Vector3 GetRayPosition(out GameObject hitObject, Vector3 startPosition)// 카메라가 바라보는 점을 리턴함
    {
        var crossHairPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, cam.farClipPlane);
        var camPoint = cam.ScreenToWorldPoint(crossHairPos);
        var dir = camPoint - startPosition;
        var hit = new RaycastHit();

        var retVal = transform.forward * 100 + transform.position;
        
        hitObject = null;

        int layer = (1 << 8) | (1 << 9);

        var hits = Physics.RaycastAll(transform.position, camPoint - transform.position, Mathf.Infinity, layer).OrderBy(h => h.distance).ToArray();
        var length = hits.Length;

        if(length > 0)
        {
            bool hitMyPlayer = false;

            var myPlayer = PlayerManager.GetMyPlayer();
            
            for (int i = 0; i < length; i++)
            {
                var nowHit = hits[i];
                var nowObject = nowHit.collider.gameObject;

                if(nowObject.CompareTag("Wall"))
                {
                    hitMyPlayer = false;
                    hit = nowHit;
                    break;
                }
                else if(nowObject.CompareTag("Player") || nowObject.CompareTag("PlayerHead"))
                {
                    if(System.Object.ReferenceEquals(myPlayer.gameObject, nowHit.transform.root.gameObject))
                    {// 카메라에서 레이케스트 하기 때문에 자신에게 데미지를 입히는 경우가 있어 그것을 방지하기 위함
                        hitMyPlayer = true;
                    }
                    else
                    {
                        hitMyPlayer = false;
                        hit = nowHit;
                        break;
                    }
                }
            }
            
            if(!hitMyPlayer)
            {
                retVal = hit.point;

                hitObject = hit.collider.gameObject;
            }
        }

        Debug.DrawRay(transform.position, retVal - transform.position, Color.red);

        return retVal;
    }

    public void Punch(float power)
    {
        vector.x -= power * Time.deltaTime;

        int sign = Random.Range(-1, 2);
        vector.y += sign * power * Time.deltaTime * 0.05f;
    }

    void LateUpdate()
    {
        if(targetObject == null)
        {
            return;
        }
        
        if(lookObject == null)
        {
            if(mouseLock)
            {
                vector.y += Input.GetAxis("Mouse X") * 150.0f * Time.deltaTime;
                vector.x -= Input.GetAxis("Mouse Y") * 150.0f * Time.deltaTime;

                nowDistance = Mathf.Lerp(nowDistance, targetDistance, Time.fixedDeltaTime * lerp);
                center.y = Mathf.Lerp(center.y, targetZoomCenterY, Time.fixedDeltaTime * lerp);

                vector.x = ClampAngle(vector.x, yMinLimit, yMaxLimit);

                transform.rotation = Quaternion.Euler(vector);
                transform.position = transform.rotation * new Vector3(center.x, center.y, -nowDistance) + targetObject.transform.position + new Vector3(0.0f, height, 0.0f);
            }
        }
        else
        {
            var direction =  lookObject.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 5.0f * Time.deltaTime);
            // transform.LookAt(lookObject.transform.position);
        }



        int layer = 1 << 8;
        
        RaycastHit hit;

        if(Physics.Raycast(targetObject.transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.position - targetObject.transform.position, out hit, nowDistance, layer))
        {
            transform.position = hit.point;
        }
    }
}
