using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserAI : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private SettingManager settingManager;
    [SerializeField]
    private FlashLightManager flashLightManager;
    [SerializeField]
    private GameOverManager gameOverManager;

    [SerializeField]
    private GameObject currStateColor = null;
    private Renderer stateRenderer;
    private Animator chaserAnimation;

    private Vector3 lastPostion;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private GameObject chaserWalkSound = null;
    [SerializeField]
    private GameObject chaserRunSound = null;

    private float canKnowTime = 3f;
    [SerializeField]
    private Transform targetPos = null;

    private float doorOpenWaitTime = 2f;

    /// <NavAgent> --------------------------------------------------------------------------------------------------------
    private NavMeshAgent navAgent;
    private NavMeshHit hit;
    ///</NavAgent> --------------------------------------------------------------------------------------------------------


    /// <FSM> ------------------------------------------------------------------------------------------------------------
    public enum ChaserState //체이서의 상태를 정리
    {
        Idle, // 체이서가 아무것도 하지않는 기본 상황
        Patrol, // 체이서가 플레이어를 인식하지 못한 상태일때 플레이어를 찾기 위해 맵안을 무작위로 패트롤
        ChasePlayerWithSound, // 체이서가 플레이어를 청각으로 인식, 추적
        ChasePlayerWithRay, // 체이서가 플레이어를 시각으로 인식, 추적
        AvoidUVLight //체이서가 플레이어의 UV라이트를 회피
    }

    public ChaserState currentState; //체이서의 현재 상태를 저장

    /// </FSM> ------------------------------------------------------------------------------------------------------------

    /// <RaycastForChase> ------------------------------------------------------------------------------------------------------------
    private Ray ray;
    /// </RaycastForChase> ------------------------------------------------------------------------------------------------------------

    /// <ChasePlayerWithRay> ------------------------------------------------------------------------------------------------------------
    private RaycastHit raycastHit;
    [SerializeField]
    private float maxRayDistance = 5f;
    [SerializeField]
    private float eyeBeamSpeedDown = 0.8f;
    [SerializeField]
    private float eyeBeamCoolForSetting = 90f;
    [SerializeField]
    private float eyeBeamRangeMin = 8f;
    private float playerNomalWalkSpeed = 0f;
    private float playerNomalRunSpeed = 0f;
    private float eyeBeamCoolForCode = 0f;
    private bool isHitEyeBeam = false;
    /// </ChasePlayerWithRay> ------------------------------------------------------------------------------------------------------------ 

    /// <Patrol> ------------------------------------------------------------------------------------------------------------ 
    private float patrolWaitTime = 2f;   // 체이서가 목적지에 도착한 후 대기하는 시간
    [SerializeField]
    private float searchRange = 50f;  // 목표 지점을 선택할 때 기준이 되는 거리
    private float minWaypointDistance = 1f; // 목적지 도착 판단거리
    /// </Patrol> ------------------------------------------------------------------------------------------------------------ 

    private void Awake()
    {
        chaserAnimation = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        GetComponent<NavMeshAgent>().speed = moveSpeed;
        chaserAnimation.SetBool("isWalk", false);
        chaserAnimation.SetBool("isRun", false);

        playerNomalWalkSpeed = player.walkSpeed;
        playerNomalRunSpeed = player.runSpeed;

        currentState = ChaserState.Idle;

        stateRenderer = currStateColor.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        CurrStateColor();
        CheckPlayerSound();
        RaycastForChase();
        UVLightHit();

        //if (Mathf.Abs(navAgent.velocity.sqrMagnitude) > 0.2f)
        //{
        //    walkAnimation.SetBool("isWalk", true);
        //}
        //else if (Mathf.Abs(navAgent.velocity.sqrMagnitude) == 0)
        //{
        //    walkAnimation.SetBool("isWalk", false);
        //}

        if (!gameOverManager.WhenDie())
        {
            switch (currentState)
            {
                case ChaserState.Idle:
                    // 아이들 상태일 때 할 행동 입력
                    Idle();
                    //moveSpeed = player.PlayerSpeed() * 0.8f;
                    chaserAnimation.SetBool("isWalk", false);
                    chaserAnimation.SetBool("isRun", false);
                    chaserWalkSound.SetActive(false);
                    chaserRunSound.SetActive(false);
                    break;
                case ChaserState.Patrol:
                    // 상태일 때 할 행동 입력
                    Patrol();
                    //moveSpeed = player.PlayerSpeed() * 0.8f;
                    chaserAnimation.SetBool("isWalk", true);
                    chaserWalkSound.SetActive(true);
                    chaserRunSound.SetActive(false);
                    break;
                case ChaserState.ChasePlayerWithSound:
                    // 청각으로 플레이어를 인식한 상태일 때 할 행동 입력
                    ChasePlayerWithSound();
                    //moveSpeed = player.PlayerSpeed() * 0.8f;
                    chaserAnimation.SetBool("isWalk", true);
                    chaserWalkSound.SetActive(true);
                    chaserRunSound.SetActive(false);
                    break;
                case ChaserState.ChasePlayerWithRay:
                    ChasePlayerWithRay();
                    //moveSpeed = player.PlayerSpeed() * 1.2f;
                    chaserAnimation.SetBool("isRun", true);
                    chaserAnimation.SetBool("isWalk", false);
                    chaserWalkSound.SetActive(false);
                    chaserRunSound.SetActive(true);
                    break;
                case ChaserState.AvoidUVLight:
                    // UV라이트를 회피 중일 때 할 행동입력
                    AvoidUVLight();
                    //moveSpeed = player.PlayerSpeed() * 1.2f;
                    chaserAnimation.SetBool("isRun", true);
                    chaserAnimation.SetBool("isWalk", false);
                    chaserWalkSound.SetActive(false);
                    chaserRunSound.SetActive(true);
                    break;
            }
        }
        else
        {
            //GetComponent<ChaserAI>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            chaserWalkSound.SetActive(false);
        }


        Debug.DrawLine(transform.position, transform.position + navAgent.velocity, Color.blue, 0.2f, false);
        //Debug.DrawLine(transform.position, hit.position, Color.yellow, 0.2f, false);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(targetPos.position, new Vector3(1f, 1f, 1f));

    }

    private void RaycastForChase() //시각으로 플레이어를 찾기 위해 레이를 쏘는 함수
    {
        float rayLength = 30f;
        float raySpreadAngle = 60f;
        Vector3 rayDirection = transform.forward;
        Vector3 rayEndPoint = transform.position + rayDirection * rayLength;
        Vector3 raySpreadVector = Random.Range(-raySpreadAngle, raySpreadAngle) * transform.right + Random.Range(-raySpreadAngle, raySpreadAngle) * transform.up;
        Vector3 raySpreadEndPoint = rayEndPoint + raySpreadVector;
        Debug.DrawLine(transform.position + Vector3.up * 1f, raySpreadEndPoint, Color.green);

        ray = new Ray(transform.position + Vector3.up * 1f, rayDirection);

        if (Physics.Raycast(ray, out raycastHit, maxRayDistance))
        {
            if (raycastHit.transform.name == "Player")
            {
                lastPostion = player.PlayerCurPosition();
                currentState = ChaserState.ChasePlayerWithRay;
            }
        }
    }

    private void ChasePlayerWithRay()
    {
        transform.LookAt(lastPostion);
        navAgent.SetDestination(lastPostion);
        if (Vector3.Distance(transform.position, player.PlayerCurPosition()) > eyeBeamRangeMin && eyeBeamCoolForCode == 0 && !isHitEyeBeam)
        {
            isHitEyeBeam = true;
            player.walkSpeed *= eyeBeamSpeedDown;
            player.runSpeed *= eyeBeamSpeedDown;
        }

        if (eyeBeamCoolForCode >= eyeBeamCoolForSetting)
        {
            eyeBeamCoolForCode = 0;
            isHitEyeBeam = false;
        }

        if (eyeBeamCoolForCode >= 10)
        {
            player.walkSpeed = playerNomalWalkSpeed;
            player.runSpeed = playerNomalRunSpeed;
        }

        if (isHitEyeBeam)
        {
            eyeBeamCoolForCode += Time.deltaTime;
        }

        if (navAgent.remainingDistance < 0.2f)
        {
            currentState = ChaserState.Patrol;
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < 5f)
        {
            canKnowTime -= Time.deltaTime;
            if (canKnowTime >= 0)
            {
                transform.LookAt(player.transform.position);
            }
        }

        transform.Rotate(0f, 30f * Time.deltaTime * 7f, 0f);
        patrolWaitTime -= Time.deltaTime;
        doorOpenWaitTime = 2f;
        if (patrolWaitTime <= 0f)
        {
            PickRandomWaypoint();
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < 5f)
        {

            canKnowTime -= Time.deltaTime;
            if (canKnowTime >= 0)
            {
                transform.LookAt(player.transform.position);
            }
        }

        if (navAgent.remainingDistance < minWaypointDistance)
        {
            currentState = ChaserState.Idle;
        }
    }

    private void PickRandomWaypoint()
    {
        if (RandomPoint(targetPos.position, searchRange, out lastPostion))
        {
            targetPos.position = lastPostion;
            patrolWaitTime = 3f; // 목적지 변경후 대기시간 초기화
            currentState = ChaserState.Patrol;
            navAgent.SetDestination(lastPostion);
        }

    }

    private void CheckPlayerSound()
    {
        if (player.isSoundOn || player.isPlayerRun)
        {
            lastPostion = player.PlayerCurPosition();
            currentState = ChaserState.ChasePlayerWithSound;
        }
    }

    private void ChasePlayerWithSound()
    {
        navAgent.SetDestination(lastPostion);
        if (navAgent.remainingDistance < 0.2f)
        {
            currentState = ChaserState.Patrol;
        }
    }

    private void UVLightHit()
    {
        if (player.rayString == transform.name && flashLightManager.isUVLightOn)
        {
            currentState = ChaserState.AvoidUVLight;
        }
    }

    private void AvoidUVLight()
    {
        lastPostion = transform.position - player.PlayerCurPosition();
        transform.LookAt(lastPostion);
        navAgent.SetDestination(transform.position + lastPostion);
        if (navAgent.remainingDistance < 2f)
        {
            currentState = ChaserState.Patrol;
        }
    }

    private void CurrStateColor()
    {
        if (currentState == ChaserState.Idle)
        {
            stateRenderer.material.color = Color.yellow;
        }
        else if (currentState == ChaserState.Patrol)
        {
            stateRenderer.material.color = Color.green;
        }
        else if (currentState == ChaserState.ChasePlayerWithRay)
        {
            stateRenderer.material.color = Color.red;
        }
        else if (currentState == ChaserState.ChasePlayerWithSound)
        {
            stateRenderer.material.color = Color.blue;
        }
    }

    public Vector3 GivePlayerPosition()
    {
        return transform.position + transform.forward * 1.5f;
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.CompareTag("Door"))
        {
            doorOpenWaitTime -= Time.deltaTime;
            if (doorOpenWaitTime <= 0)
            {
                _other.gameObject.GetComponent<Animator>().SetBool("isOpen", true);
                doorOpenWaitTime = 2f;
            }
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.CompareTag("Door"))
        {
            _other.gameObject.GetComponent<Animator>().SetBool("isOpen", false);
            doorOpenWaitTime = 2f;
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }


}