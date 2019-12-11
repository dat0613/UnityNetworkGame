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

    public float warpDistance = 1.0f;

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

    private Vector3 lastTargetPosition;
    private Vector3 targetPosition;
    private Vector3 sendChestRotation;
    private Vector3 targetChestRotation;
    private Quaternion ChestQuaternion;

    public float limit;

    bool allow = false;

    private int nowTimeStamp = 0;
    private int lastTimeStamp = 0;

    float lastSyncLocalTime = 0.0f;
    float nowSyncLocalTime = 0.0f;

    int rotateSign = 0;
    float lastY = 0.0f;

    private bool zoomMode = false;

    private List<bool> lastAnimationState;

    private Vector3 rotatepersecond;

    public int kill;
    public int death;

    float timeDifference = 0.0f;
    float rotateTime = 0.0f;// 가장 최근에 동기화가 일어났던 때로 부터의 시간
    private bool isKeyInput = false;

    public Material translucentMaterial;
    public Material redTeamMaterial;
    public Material blueTeamMaterial;

    [SerializeField]
    List<SkinnedMeshRenderer> renderers;

    List<int> layerList;// 자식들의 레이어
    int layer;// 이 오브젝트의 레이어

    List<Material> materialsList;
    Material gunMaterial;

    public void SetAsKiller(bool option)
    {
        if(option)
        {// 해당 플레이어를 킬러로 지정하고 벽에 투시되게끔 함
            SetLayer(10);
            SetMaterial(translucentMaterial);
        }
        else
        {// 벽에 투시되게끔 했던 설정을 초기화 시킴
            LoadLayer();
            LoadMaterial();
        }
    }

    private void SetLayer(int layer)
    {
        var childCount = transform.childCount;
        
        for(int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = layer;
        }

        gameObject.layer = layer;
    }

    private void SaveLayer()
    {
        var childCount = transform.childCount;
        layerList = new List<int>(childCount);
        
        for(int i = 0; i < childCount; i++)
        {
            layerList.Add(transform.GetChild(i).gameObject.layer);
        }

        layer = gameObject.layer;
    }

    public void LoadLayer()
    {
        var childCount = layerList.Count;

        for(int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = layerList[i];
        }

        gameObject.layer = layer;
    }

    private void SetMaterial(Material material)
    {
        int rendererCount = renderers.Count;

        for(int i = 0; i < rendererCount; i++)
        {
            renderers[i].material = material;
        }

        grip.gun.SetMaterial(material);
    }

    private void SaveMaterial()
    {
        int rendererCount = renderers.Count;
        materialsList = new List<Material>();

        for(int i = 0; i < rendererCount; i++)
        {
            materialsList.Add(renderers[i].material);
        }

        gunMaterial = grip.gun.GetMaterial();
    }

    public void LoadMaterial()
    {
        int rendererCount = renderers.Count;

        for(int i = 0; i < rendererCount; i++)
        {
            renderers[i].material = materialsList[i];
        }

        grip.gun.SetMaterial(gunMaterial);
    }

    void OnDestroy()
    {
        PlayerManager.DelPlayer(this);
        if(grip.gun != null)
            Destroy(grip.gun.gameObject);
    }

    public void OnInstantiate(int team, int state, int maxHP, string nickName)
    {// 원래 게임 룸에 있던 객체들의 동기화
        Team changeTeam = (Team)team;
        State changeState = (State)state;

        this.playerName = nickName;

        if(isMine)
            UIManager.Instance.SetNickName(nickName);
        
        ChangeTeam(changeTeam);
        ChangeState(changeState);
        this.maxHP = maxHP;
    }

    void Awake()
    {
        chestTransform = animator.GetBoneTransform(HumanBodyBones.Chest);
        grip = GetComponent<GunGrip>();
        grip.gun = Instantiate(GunPrefab);
        grip.gun.leftHand = leftHand;
        nowHP = maxHP;

        SaveLayer();
        SaveMaterial();
    }

    public void Respawn(Vector3 position, int hp, int team)
    {
        SoundManager.Instance.PlaySound("Respawn", position, 8.0f);
        
        if(isMine)
        {
            transform.position = position;
            AutoSync();
        }
        
        nowHP = hp;

        Team respawnTeam = (Team)team;
        if(respawnTeam == Team.Red)
        {
            SetMaterial(redTeamMaterial);
        }

        if(respawnTeam == Team.Blue)
        {
            SetMaterial(blueTeamMaterial);
        }

        SaveMaterial();

        ChangeTeam((Team)team);
        ChangeState(State.Alive);

        if(isMine)
        {
            UIManager.Instance.SetGameUI(this.maxHP, this.nowHP, grip.gun.maxOverheat, grip.gun.nowOverheat);
            UIManager.Instance.ViewGameUI();
        }
    }

    public override void OnSetID(int objectID)
    {
        if (isMine)
        {
            FollowCamera.Instance.targetObject = gameObject;
            InvokeRepeating("AutoSync", 0.0f, 0.2f);
            InitAnimationSyncingSystem();
            ChangeTeam(Team.Spectator);
        }

        PlayerManager.AddPlayer(this);
    }

    public void SyncPosition(Vector3 position, Vector3 chestRotation, int rotateSign, int timeStamp)
    {
        if(Vector3.Distance(position, transform.position) > warpDistance)
        {
            transform.position = position;
        }

        lastSyncLocalTime = nowSyncLocalTime;
        nowSyncLocalTime = Time.time;

        lastTargetPosition = targetPosition;
        targetPosition = position;
        targetChestRotation = chestRotation;

        timeDifference = timeStamp - lastTimeStamp;

        lastTimeStamp = nowTimeStamp;
        nowTimeStamp = timeStamp;

        Vector3 rotationGap = chestRotation - chestTransform.rotation.eulerAngles;

        if (rotationGap.y > 180.0f)      // 각도가 갑자기 확 돌아가는것을 막기위한 작업
            rotationGap.y -= 360.0f;
        if (rotationGap.y < -180.0f)
            rotationGap.y += 360.0f;
        if (rotationGap.x > 180.0f)
            rotationGap.x -= 360.0f;
        if (rotationGap.x < -180.0f)
            rotationGap.x += 360.0f;
        if (rotationGap.z > 180.0f)
            rotationGap.z -= 360.0f;
        if (rotationGap.z < -180.0f)
            rotationGap.z += 360.0f;

        rotateTime = 0.0f;

        Vector3 rotatePer1ms = rotationGap / timeDifference;
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
            headBox.tag = tag = "Untagged";
        }
        controller.enabled = state;
    }

    void SetVisible(bool visible)// 렌더링 끄기
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            renderers[i].enabled = visible;
        }

        if(grip == null)
        {
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
            grip.gun.enabled = true;
            if(isMine)
                FollowCamera.Instance.SetKiller(null);
            grip.gun.OverheatReset();// 과열 상태를 초기화 시킴
            break;

            case State.Die:
            SetCollision(false);
            grip.gun.gameObject.SetActive(false);
            animator.SetBool("Die", true);
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
            FollowCamera.Instance.SetKiller(killer);
        }
        if(killer.isMine)
        {// 내가 죽임
            UIManager.Instance.KillFeedBack(playerName);
        }
        UIManager.Instance.AddKillLog(shooterID, objectId, isHead);
    }

    public void DieInformation(int shooterID, int myKillCount, int killersKillCount, int respawnTime, int timeStamp)
    {
        var killer = PlayerManager.GetPlayer(shooterID);
        if(killer == null)
            return;

        float latency = MinNetUser.ServerTime - timeStamp;// 네트워크 상에서 지연된 시간을 계산함
        float respawnTimef = (respawnTime - latency) * 0.001f;// 단위를 초 단위로 바꿈

        UIManager.Instance.SetDieUI(killer.playerName, myKillCount, killersKillCount, respawnTimef);
        UIManager.Instance.ViewDieUI();
    }

    public void PlayerRestore(Vector3 position)
    {

    }

    public void HitSuccess(bool isHead, int damage)
    {
        UIManager.Instance.HitFeedBack(damage, isHead);

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
            UIManager.Instance.AddHitCircle(shotPosition);
            UIManager.Instance.ViewEffect(damage);
            UIManager.Instance.UpdateGameUI(nowHP, grip.gun.nowOverheat);
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
        RPCudp("SyncPosition", MinNetRpcTarget.Others, transform.position, sendChestRotation, rotateSign, MinNetUser.ServerTime);
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

    public void PlayRandomFootstep()
    {
        string clipName = "";

        if(Random.Range(0, 2) == 0)
            clipName = "FootStep1";
        else
            clipName = "FootStep2";

        SoundManager.Instance.PlaySound(clipName, transform.position, 10.0f, 0.1f);
    }

    void PlayerInput()
    {
        if(!allow)
            return;
            
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
        UIManager.Instance.AddChat(chat, new Color(r, g, b, a));
    }

    public void SyncScore(int kill, int death)
    {
        this.kill = kill;
        this.death = death;
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

                UIManager.Instance.UpdateGameUI(nowHP, grip.gun.nowOverheat);// 죽었을때는 게임 ui를 숨길것 이므로 살아있을때만 업데이트 하면 댐
            }

            if (state == State.Alive && (zoomMode || isKeyInput))
            {
                transform.rotation = Quaternion.Euler(0.0f, FollowCamera.Instance.vector.y, 0.0f);
            }
        }
        else
        {
            // transform.position += movepersecond * Time.fixedDeltaTime;
            var timedif = 0.2f;
            var ratio = 0.0f;
            var nowtime = Time.time - timedif;

            if(timedif < 0.01f)
                ratio = 0.0f;
            else
                ratio = (nowtime - lastSyncLocalTime) / timedif; 

            var movepos = (targetPosition - lastTargetPosition);
            var pos = ratio * movepos + lastTargetPosition;
            transform.position = pos;

            if (zoomMode || isKeyInput)
            {
                transform.rotation = Quaternion.Euler(0.0f, ChestQuaternion.eulerAngles.y - setting.y, 0.0f);
            }
        }
    }

    void LateUpdate()
    {
        if (isMine)
        {
            AnimationSyncingSystem();

            float degree = chestTransform.eulerAngles.y;
            
            lastY = degree;

            if(state == State.Alive)
            {
                chestTransform.LookAt(Camera.main.transform.forward * 10 + Camera.main.transform.position);
                chestTransform.rotation = chestTransform.rotation * Quaternion.Euler(setting);
            }

            sendChestRotation = chestTransform.rotation.eulerAngles;
        
            rotateSign = (int)(sendChestRotation.y - lastY);
            
            // Debug.Log(rotateSign + ", " + sendChestRotation.y + ", " + lastY);
            if(rotateSign < 0)
            {
                // Debug.Log("오른쪽으로 회전");
            }
            if(rotateSign > 0)
            {
                // Debug.Log("왼쪽으로 회전");
            }
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

    public void SetMaxHP(int maxHP)
    {
        this.maxHP = nowHP = maxHP;

        if(isMine)
        {// 최대 체력이 갱신되었으므로 UI에 반영 시킴
            UIManager.Instance.SetGameUI(this.maxHP, this.nowHP, grip.gun.maxOverheat, grip.gun.nowOverheat);
        }
    }

    public void SetControllAllow(bool allow)
    {
        this.allow = allow;
    }
}