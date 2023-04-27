using UnityEngine;

[RequireComponent(typeof(CharacterController))] // 컴포넌트 자동 추가!

public class Player : MonoBehaviour
{
    [SerializeField]
    private SettingManager settingManager;
    [SerializeField]
    private ChaserAI chaserAI;

    [SerializeField]
    private GameOverManager gameOverManager;

    [Header("Speed")]

    [SerializeField]
    private float normalSpeed = 5.0f;  // 기본 스피드
    [SerializeField]
    public float walkSpeed = 5.0f;  // 걷기
    [SerializeField]
    public float runSpeed = 10.0f; // 달리기
    [SerializeField]
    private float jump = 10.0f; // 점프
    [SerializeField]
    private float gravity = 9.81f;

    private float runCoolTime = 0f;
    private float canRumTime = 0f;

    [Space(10f)]

    private CharacterController player = null;
    private Vector3 MoveDir = Vector3.zero;
    private Animator anim;
    private float mouseX = 0.0f;
    private float mouseXSpeed = 3.0f;

    [SerializeField]
    private GameObject walkSound = null;
    [SerializeField]
    private GameObject runSound = null;

    [System.NonSerialized]
    public bool isSoundOn = false;
    [System.NonSerialized]
    public bool isPlayerRun = false;
    [Space(10f)]
    [Header("Ray")]

    [SerializeField]
    private Camera playerCamera = null;
    private Ray ray;
    private RaycastHit rayHit;
    [SerializeField]
    private float maxRay = 5f;
    [System.NonSerialized]
    public string rayString = null;


    private bool isWalkSoundOn = false;
    private bool isRunSoundOn = false;

    private bool isCanRun = true;

    private void Awake()
    {
        player = GetComponent<CharacterController>();
        anim = transform.GetComponent<Animator>();

        walkSound.SetActive(false);
        runSound.SetActive(false);
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        PlayerRay();
        PlayerMove();
        PlayerRotate();
        PlayWalktSound();
        PlayWRuntSound();

        if (runCoolTime > 0f)
        {
            RunCoolTime();
        }

        if (gameOverManager.WhenDie())
        {
            transform.position = chaserAI.GivePlayerPosition();
            playerCamera.GetComponent<CameraManager>().enabled = false;
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Player>().enabled = false;
        }


    }


    private void PlayWalktSound() 
    {
        if (isWalkSoundOn)
        {
            walkSound.SetActive(true);
        }
        else
        {
            walkSound.SetActive(false);
        }

        // Debug.Log("runCoolTime : " + runCoolTime);
    }

    private void PlayWRuntSound()
    {
        if (isRunSoundOn)
        {
            runSound.SetActive(true);
        }
        else
        {
            runSound.SetActive(false);
        }
    }

    private void PlayerMove()
    {
        if (player.isGrounded)
        {
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            MoveDir = transform.TransformDirection(MoveDir.normalized);
            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환

            if (MoveDir == Vector3.zero)
            {
               
                isWalkSoundOn = false;
                isRunSoundOn = false;
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", false);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift) && isCanRun)
                {
                        isPlayerRun = true;
                        isRunSoundOn = true;
                        isWalkSoundOn = false;
                        normalSpeed = runSpeed;
                        anim.SetBool("isWalk", false);
                        anim.SetBool("isRun", true);
                        Stoprun();
                }
                else
                {
                    anim.SetBool("isWalk", true);
                    anim.SetBool("isRun", false);
                    isWalkSoundOn = true;
                    isRunSoundOn = false;
                    isSoundOn = false;
                    isPlayerRun = false;
                    normalSpeed = walkSpeed;
                }
            }

            MoveDir *= normalSpeed;

            if (Input.GetButton("Jump"))
            {
                MoveDir.y = jump;
            }
        }
        MoveDir.y -= gravity * Time.deltaTime;
        player.Move(MoveDir * Time.deltaTime);
    }

    private void Stoprun()
    {
        canRumTime += Time.deltaTime;
        if (canRumTime >= 5f)
        {
            isCanRun = false;
            runCoolTime = 10f;
        }
    }

    private void RunCoolTime()
    {
        runCoolTime -= Time.deltaTime;
        if (runCoolTime <= 0)
        {
            isCanRun = true;
            runCoolTime = 0;
            canRumTime = 0;
        }
    }

    private void PlayerRotate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseXSpeed;
        transform.eulerAngles = new Vector3(0, mouseX, 0);
    }

    private void PlayerRay()
    {
        ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out rayHit, maxRay))
        {
            Debug.DrawLine(ray.origin, rayHit.point, Color.green);
            rayString = rayHit.transform.name;
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.direction * 100, Color.red);
            rayString = null;
        }
    }

    public Vector3 PlayerCurPosition()
    {
        return transform.position;
    }

    public float PlayerSpeed()
    {
        return normalSpeed;
    }

    public string DoorNameCheck()
    {
        return rayString;
    }
}