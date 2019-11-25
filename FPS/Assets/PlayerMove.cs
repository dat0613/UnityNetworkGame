using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MinNetforUnity;

public class PlayerMove : MonoBehaviourMinNet
{
    public enum Team{ Red, Blue, Spectator, Individual, None };
    public enum State { Alive, Die };
    
    public string playerName = "";

    public Team team = Team.Spectator;
    public State state = State.Die;

    [SerializeField]
    private GameObject headBox;

    [SerializeField]
    private CharacterController controller;

    private Vector3 keyInput = new Vector3(0.0f, 0.0f, 0.0f);
    public Animator animator;
    private Transform chestTransform;
    public Vector3 setting;

    public float gravityPower = 0.0f;

    private GunGrip grip = null;
    public Gun GunPrefab;
    public Transform leftHand;

    public int maxHP = 150;
    public int nowHP;

    private Vector3 targetPosition;
    private Vector3 sendChestRotation;
    private Vector3 targetChestRotation;
    private Quaternion ChestQuaternion;

    private int lastTimeStamp = 0;

    private bool zoomMode = false;

    private List<bool> lastAnimationState;

    private Vector3 movepersecond;
    private Vector3 rotatepersecond;

    float timeDifference = 0.0f;
    float rotateTime = 0.0f;// 가장 최근에 동기화가 일어났던 때로 부터의 시간
    private bool isKeyInput = false;

    [SerializeField]
    List<SkinnedMeshRenderer> renderers;

    void OnDestroy()
    {
        PlayerManager.DelPlayer(this);
        if(grip.gun != null)
            Destroy(grip.gun.gameObject);
    }

    public void OnInstantiate(int team, int state, int nowHP)
    {// 원래 게임 룸에 있던 객체들의 동기화
        Team changeTeam = (Team)team;
        State changeState = (State)state;

        Debug.Log("동기화 : " + changeTeam.ToString() + ", " + changeState.ToString());
        
        ChangeTeam(changeTeam);
        ChangeState(changeState);
        this.nowHP = nowHP;
    }

    void Awake()
    {
        chestTransform = animator.GetBoneTransform(HumanBodyBones.Chest);
        grip = GetComponent<GunGrip>();
        grip.gun = Instantiate(GunPrefab);
        grip.gun.leftHand = leftHand;
        nowHP = maxHP;
    }

    void Start()
    {
        if (isMine)
        {
            FollowCamera.Instance.targetObject = gameObject;
            InvokeRepeating("AutoSync", 0.0f, 0.1f);
            InitAnimationSyncingSystem();
            ChangeTeam(Team.Spectator);
        }
        else
        {
            PlayerManager.AddPlayer(this);
        }
    }

    public void Respawn(Vector3 position, int hp, int team)
    {
        transform.position = position;
        nowHP = hp;
        ChangeTeam((Team)team);
        ChangeState(State.Alive);
    }

    public override void OnSetID(int objectID)
    {
        if(isMine)
        {
            PlayerManager.AddPlayer(this);
        }
    }

    public void SyncPosition(Vector3 position, Vector3 chestRotation, int timeStamp)
    {
        targetPosition = position;
        targetChestRotation = chestRotation;

        timeDifference = timeStamp - lastTimeStamp;
        lastTimeStamp = timeStamp;

        Vector3 rotationGap = chestRotation - chestTransform.rotation.eulerAngles;

        if(rotationGap.y > 180.0f)      // 각도가 갑자기 확 돌아가는것을 막기위한 작업
            rotationGap.y -= 360.0f;
        if(rotationGap.y < -180.0f)
            rotationGap.y += 360.0f;
        if(rotationGap.x > 180.0f)
            rotationGap.x -= 360.0f;
        if(rotationGap.x < -180.0f)
            rotationGap.x += 360.0f;
        if(rotationGap.z > 180.0f)
            rotationGap.z -= 360.0f;
        if(rotationGap.z < -180.0f)
            rotationGap.z += 360.0f;

        rotateTime = 0.0f;

        Vector3 rotatePer1ms = rotationGap / timeDifference;

        Vector3 positionGap = position - transform.position;
        Vector3 movePer1ms = positionGap / timeDifference;

        movepersecond = movePer1ms * 1000;
        rotatepersecond = rotatePer1ms * 1000;
    }

    void SetCollision(bool state)
    {
        if(state)
        {
            headBox.tag = "PlayerHead";
            tag = "Player";
        }
        else
        {
            // Debug.Log("여기까지옴");
            headBox.tag = tag = "Untagged";
        }
        // controller.isTrigger = !state;
    }

    void SetVisible(bool visible)// 렌더링 끄기
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            renderers[i].enabled = visible;
        }

        if(grip == null)
        {
            Debug.Log("grip이 없대 isMine : " + isMine.ToString());
        }
        else
        {
            grip.gun.SetVisible(visible);
        }
    }

    public void ChangeTeam(Team team)
    {
        if(this.team == team)
            return;

        Debug.Log(objectId.ToString() + " 의 팀을 " + team.ToString() + " 으로 바꿈, isMine : " + isMine.ToString());

        switch (team)
        {
            case Team.Blue:
            case Team.Red:
            case Team.Individual:// 개인전
            SetCollision(true);
            SetVisible(true);
                break;

            case Team.Spectator:// 관전자
            SetCollision(false);
            SetVisible(false);
                break;
        }

        this.team = team;
    }

    public void ChangeState(State state)
    {
        if(this.state == state)
        {
            return;
        }
        
        switch (state)
        {
            case State.Alive:
            SetCollision(true);
            grip.gun.gameObject.SetActive(true);
            animator.SetBool("Die", false);
            controller.enabled = true;
            grip.gun.enabled = true;
            if(isMine)
                FollowCamera.Instance.lookObject = null;
            break;

            case State.Die:
            SetCollision(false);
            grip.gun.gameObject.SetActive(false);
            animator.SetBool("Die", true);
            controller.enabled = false;
            grip.gun.enabled = false;
            break;
        }

        this.state = state;
    }

    public void PlayerDie(int shooterID, string shooterName, bool isHead)
    {// 죽음
        ChangeState(State.Die);

        var killer = PlayerManager.GetPlayer(shooterID);
        if(killer == null)
            return;

        if(isMine)
        {// 내가 죽음
            Debug.Log(killer.gameObject);
            Debug.Log("쥬ㅜㄱ음");
            FollowCamera.Instance.lookObject = killer.gameObject;
        }
        if(killer.isMine)
        {// 내가 죽임
            CrossHair.Instance.KillFeedBack();
        }
        UiManager.Instance.AddKillLog(shooterID, objectId, isHead);
    }

    public void PlayerRestore(Vector3 position)
    {

    }

    public void HitSuccess(bool isHead, int damage)
    {
        CrossHair.Instance.HitFeedBack(damage, isHead);

        if(isHead)
        {

        }
        else
        {

        }
    }

    public void HitCast(Vector3 shotPosition, int damage, bool isHead)
    {
        nowHP -= damage;
        if (nowHP <= 0)
            nowHP = 0;

        if(isMine)
        {
            Debug.Log("내가 맞앗당");
            UiManager.Instance.AddHitCircle(shotPosition);
        }
    }

    public void SyncZoom(bool zoom)
    {
        zoomMode = zoom;
    }

    public void SyncKeyInput(bool isInput)
    {
        isKeyInput = isInput;
    }

    void AutoSync()
    {
        RPC("SyncPosition", MinNetRpcTarget.Others, transform.position, sendChestRotation, MinNetUser.ServerTime);
    }

    public void ShotSync(int punch, Vector3 endPoint, Vector3 shotPosition, Vector3 muzzlePosition)
    {
        if(isMine)
        {
            FollowCamera.Instance.Punch(punch);
        }
        
        grip.gun.CreateTrail(endPoint, muzzlePosition, isMine);
        // transform.position = shotPosition;
    }

    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!zoomMode)
            {
                zoomMode = true;
                RPC("SyncZoom", MinNetRpcTarget.Others, zoomMode);
            }
        }
        else
        {
            if (zoomMode)
            {
                zoomMode = false;
                RPC("SyncZoom", MinNetRpcTarget.Others, zoomMode);
            }
        }
        FollowCamera.Instance.SetZoom(zoomMode);

        keyInput = Vector3.zero;
        bool isRun = false;

        if (Input.GetKey(KeyCode.W))
        {
            keyInput.z += 1.0f;

            if (Input.GetKey(KeyCode.LeftShift))
                isRun = true;
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            RPC("HitSync", MinNetRpcTarget.Server, objectId, transform.position + Vector3.one * 0.1f, transform.position + Vector3.one * 0.1f, true);
        }

        if (!isRun)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                GameObject obj;
                var endPoint = FollowCamera.Instance.GetRayPosition(out obj, grip.gun.muzzleTransform.position);
                if (grip.gun.Shot(obj))
                {
                    int punch = Random.Range(40, 60);
                    RPC("ShotSync", MinNetRpcTarget.All, punch, endPoint, transform.position, grip.gun.muzzleTransform.position);
                    
                    if(obj != null)
                    {
                        bool isPlayer = obj.CompareTag("Player");
                        bool isHead = obj.CompareTag("PlayerHead");

                        if(isPlayer || isHead)
                        {// 플레이어 명중
                            GameObject hitObject = null;
                            PlayerMove component = null;

                            if(isHead)
                            {
                                hitObject = obj.transform.root.gameObject;
                            }
                            else
                            {
                                hitObject = obj;
                            }

                            component = hitObject.GetComponent<PlayerMove>();
                            RPC("HitSync", MinNetRpcTarget.Server, component.objectId, hitObject.transform.position, transform.position, isHead);
                        }
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.S))
            keyInput.z -= 1.0f;

        if (Input.GetKey(KeyCode.A))
            keyInput.x -= 1.0f;

        if (Input.GetKey(KeyCode.D))
            keyInput.x += 1.0f;

        if (keyInput.z > 0.1f)
        {// 앞으로 가는중
            animator.SetBool("GoForward", true);
            // 뛰는중
            animator.SetBool("IsRun", isRun);
        }
        else
        {
            animator.SetBool("GoForward", false);
        }

        if (keyInput.z < -0.1f)
        {// 뒤로 가는중
            animator.SetBool("GoBack", true);
        }
        else
        {
            animator.SetBool("GoBack", false);
        }

        if (keyInput.x > 0.1f)
        {// 오른쪽으로 가는 중
            animator.SetBool("GoRight", true);
        }
        else
        {
            animator.SetBool("GoRight", false);
        }

        if (keyInput.x < -0.1f)
        {// 왼쪽으로 가는중
            animator.SetBool("GoLeft", true);
        }
        else
        {
            animator.SetBool("GoLeft", false);
        }

        if (Mathf.Abs(keyInput.x) < 0.01f && Mathf.Abs(keyInput.z) < 0.01f)
        {
            animator.SetBool("IsRun", false);
            animator.SetBool("GoForward", false);
            animator.SetBool("GoRight", false);
            animator.SetBool("GoBack", false);
            animator.SetBool("GoLeft", false);
            animator.SetBool("IsRun", false);

            if(isKeyInput)
            {
                isKeyInput = false;
                RPC("SyncKeyInput", MinNetRpcTarget.Others, isKeyInput);
            }
        }
        else
        {
            if(!isKeyInput)
            {
                isKeyInput = true;
                RPC("SyncKeyInput", MinNetRpcTarget.Others, isKeyInput);
            }
        }

        float speed = 0.0f;

        if (isRun)
            speed = 8.0f;
        else
            speed = 4.0f;

        var force = transform.rotation * keyInput.normalized * speed * Time.fixedDeltaTime;

        if(!controller.isGrounded)
        {
            force.y -= gravityPower * Time.fixedDeltaTime;
        }

        controller.Move(force);
    }

    void InitAnimationSyncingSystem()
    {
        lastAnimationState = new List<bool>();

        var parameters = animator.parameters;

        for (int i = 0; i < animator.parameterCount; i++)
        {
            lastAnimationState.Add(parameters[i].defaultBool);
        }
    }

    void AnimationSyncingSystem()
    {
        var parameters = animator.parameters;

        for (int i = 0; i < lastAnimationState.Count; i++)
        {
            string name = parameters[i].name;
            // Debug.Log(name);
            bool nowState = animator.GetBool(name);
            if (lastAnimationState[i] != nowState)
            {// 애니메이션에 변경이 생김
                RPC("SyncAnimationState", MinNetRpcTarget.Others, name, nowState, MinNetUser.ServerTime);
                lastAnimationState[i] = nowState;
            }
        }
    }

    public void SyncAnimationState(string stateName, bool state, int timeStamp)
    {
        animator.SetBool(stateName, state);
        float timeDifference = (float)(MinNetUser.ServerTime - timeStamp) * 0.001f;
        animator.Update(timeDifference);// 타임스탬프를 이용하여 네트워크 상에서 지연된 시간만큼 애니메이션을 스킵함
    }

    public void SendChatting(string chat)
    {
        RPC("Chat", MinNetRpcTarget.Server, chat);
    }

    public void Chat(string chat, float r, float g, float b, float a)// 채팅 내용과 팀에 알맞은 색상 값을 받음
    {
        UiManager.Instance.AddChat(chat, new Color(r, g, b, a));
    }

    void FixedUpdate()
    {
        if (isMine)
        {
            if(state == State.Alive)
            {
                if(team != Team.Spectator)
                {
                    if(InputManager.Instance.Focus == InputManager.InputFocus.CharacterControl)
                        PlayerInput();
                }
                else
                {// 관전모드 업데이트

                }
            }

            if (state == State.Alive && (zoomMode || isKeyInput))
            {
                transform.rotation = Quaternion.Euler(0.0f, FollowCamera.Instance.vector.y, 0.0f);
            }
        }
        else
        {
            transform.position += movepersecond * Time.fixedDeltaTime;

            if (zoomMode || isKeyInput)
                transform.rotation = Quaternion.Euler(0.0f, ChestQuaternion.eulerAngles.y, 0.0f);
        }
    }

    void LateUpdate()
    {
        if (isMine)
        {
            AnimationSyncingSystem();

            if(state == State.Alive)
            {
                chestTransform.LookAt(Camera.main.transform.forward * 10 + Camera.main.transform.position);
                chestTransform.rotation = chestTransform.rotation * Quaternion.Euler(setting);
            }

            sendChestRotation = chestTransform.rotation.eulerAngles;
        }
        else
        {
            if (timeDifference > rotateTime)
            {
                ChestQuaternion.eulerAngles += rotatepersecond * Time.deltaTime;
            }
            else
            {
                ChestQuaternion = Quaternion.Euler(targetChestRotation);
            }
            
            chestTransform.rotation = ChestQuaternion;

            rotateTime += Time.deltaTime;
        };
    }
}