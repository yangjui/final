using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Vector3 lastPostion;

    [SerializeField]
    private Player player = null;
    [SerializeField]
    private FlashLightManager flashLightManager = null;

    [SerializeField]
    private float moveSpeed = 5f;

    [Space(10f)]
    [Header("Ray")]

    private Ray ray;
    private float rayHitTime = 0;

    [SerializeField]
    private float maxRay = 5f;
    [SerializeField]
    private float rayHitTimeForStopMove = 3f;

    [Space(10f)]
    [Header("EyeBeam")]

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

    private Animator walkAnimation;

    private bool avoidUVLight = false;

    ///////////////////////////////////////////////////////////패트롤//////////////////////////////////////////////////////
    private float patrolSpeed = 2f;   // 체이서의 이동 속도
    private float patrolWaitTime = 4f;   // 체이서가 목적지에 도착한 후 대기하는 시간
    public float searchRange = 10f;  // 목표 지점을 선택할 때 기준이 되는 거리
    private float minWaypointDistance = 0.001f; // 목적지 도착 판단거리
    private Transform[] waypoints;   // 이동할 목표 지점들
    private int numWaypoints = 5;
    private float radius = 10f;
    private int currentWaypoint = 0;    // 현재 목표 지점의 인덱스
    private bool isPatrolling = false;  // 체이서가 현재 패트롤 중인지 여부
    private bool isWaiting = true;     // 체이서가 목표 지점에 도착하여 대기 중인지 여부
    //private float waitTimer = 0f;       // 대기 시간 카운터
    private bool isChasePlayerWithSound = false;
    private bool isChasePlayerWithRay = false;
    private NavMeshHit hit;

    ///////////////////////////////////////////////////////////패트롤 끝//////////////////////////////////////////////////////

    private void Awake()
    {
        walkAnimation = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        GetComponent<NavMeshAgent>().speed = moveSpeed;
        walkAnimation.SetBool("isWalk", false);

        playerNomalWalkSpeed = player.GetComponent<Player>().walkSpeed;
        playerNomalRunSpeed = player.GetComponent<Player>().runSpeed;

        //transform.GetComponent<Animator>().enabled = false;
        //transform.GetComponent<Chaser>().enabled = false;
  
    }


    private void Update()
    {
        Debug.Log("isPatrolling : " + isPatrolling);
        Debug.Log("isWaiting : " + isWaiting);
        //Debug.Log("waitTimer : " + waitTimer);
        //Debug.Log(avoidUVLight);
        Debug.DrawLine(transform.position, transform.position + navAgent.velocity, Color.blue, 0.2f, false);
        Debug.DrawLine(transform.position, hit.position, Color.yellow, 0.2f,false);
        if (!avoidUVLight)
        {
            ChasePlayerWithSound();
            ChasePlayerWithRay();
            SearchAround();
        }

        UVLightHit();

        if (Mathf.Abs(navAgent.velocity.magnitude) > 0.2f)
        {
            walkAnimation.SetBool("isWalk", true);
        }
        else if (Mathf.Abs(navAgent.velocity.magnitude) == 0)
        {
            walkAnimation.SetBool("isWalk", false);
        }

        if (avoidUVLight)
        {
            isChasePlayerWithRay = false;
            isChasePlayerWithSound = false;
            Vector3 direction = transform.position - player.PlayerCurPosition();
            navAgent.SetDestination(transform.position + direction.normalized * 5f);
        }
        else
        {
            // 패트롤 중이면서 목적지에 도착한 경우
            if (isPatrolling &&
                navAgent.remainingDistance < minWaypointDistance)
            {
                isWaiting = true;
                //waitTimer = patrolWaitTime;
            }

            // 대기 중인 경우
            if (isWaiting)
            {
                isPatrolling = false;
                transform.Rotate(0f, 30f * Time.deltaTime * 3f, 0f);
                patrolWaitTime -= Time.deltaTime;
                if (patrolWaitTime <= 0f)
                {
                    PickRandomWaypoint();
                    patrolWaitTime = 4f;
                    isWaiting = false;
                }
            }


            //패트롤 중이 아니라 적을 추격중인 경우
            if (!isWaiting && (isChasePlayerWithRay || isChasePlayerWithSound))
            {
                isPatrolling = false;
                if (navAgent.remainingDistance < 0.2f)
                {
                    //waitTimer = patrolWaitTime;
                    isChasePlayerWithRay = false;
                    isChasePlayerWithSound = false;
                    isWaiting = true;
                }
            }
        }
    }

    private void PickRandomWaypoint()
    {
        // 무작위로 위치를 선택
        Vector3 randomPosition = Random.insideUnitSphere * searchRange + transform.position;
        
        // NavMesh 위의 무작위 지점 선택
        if (NavMesh.SamplePosition(randomPosition, out hit, searchRange, NavMesh.AllAreas))
        {
            navAgent.SetDestination(hit.position);
            isPatrolling = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    private void UVLightHit()
    {
        float nomalChaserSpeed = moveSpeed;
        if (player.GetComponent<Player>().rayString == transform.name && flashLightManager.isUVLightOn)
        {
            rayHitTime += Time.deltaTime;
            avoidUVLight = true;
            searchRange = 3f;
            navAgent.speed = 30;
            PickRandomWaypoint();

            if (rayHitTime >= rayHitTimeForStopMove)
            {
                avoidUVLight = false;
                navAgent.isStopped = true;
                walkAnimation.SetBool("isWalk", false);
                Invoke("MoveAgain", 3f);
            }
        }
        else if (navAgent.remainingDistance < 1f && player.GetComponent<Player>().rayString != transform.name)
        {
            avoidUVLight = false;
            navAgent.speed = nomalChaserSpeed;
            searchRange = 20f;
        }
    }

    private void OnCollisionEnter(Collision _other)
    {
        if (_other.collider.CompareTag("FloorBall"))
        {
            navAgent.isStopped = true;
            walkAnimation.SetBool("isWalk", false);
            Invoke("MoveAgain", 5f);
        }
    }

    private void MoveAgain()
    {
        rayHitTime = 0;
        navAgent.isStopped = false;
        walkAnimation.SetBool("isWalk", true);
    }

    private void ChasePlayerWithSound()
    {
        if (player.GetComponent<Player>().isSoundOn)
        {
            isChasePlayerWithSound = true;
            navAgent.SetDestination(player.PlayerCurPosition());
            lastPostion = player.PlayerCurPosition();
        }
    }

    private void ReturnNomalPlayerSpeed()
    {
        if (eyeBeamCoolForCode >= 10)
        {
            player.GetComponent<Player>().walkSpeed = playerNomalWalkSpeed;
            player.GetComponent<Player>().runSpeed = playerNomalRunSpeed;
        }
    }

    private void SearchAround()
    {
        if (Vector3.Distance(transform.position, lastPostion) < 0.1f)
        {
            transform.Rotate(0f, 30f * Time.deltaTime, 0f);
        }
    }

    private void ChasePlayerWithRay()
    {
        float rayLength = 30f;
        float raySpreadAngle = 10f;
        Vector3 rayDirection = transform.forward;
        Vector3 rayEndPoint = transform.position + rayDirection * rayLength;
        Vector3 raySpreadVector = Random.Range(-raySpreadAngle, raySpreadAngle) * transform.right + Random.Range(-raySpreadAngle, raySpreadAngle) * transform.up;
        Vector3 raySpreadEndPoint = rayEndPoint + raySpreadVector;
        Debug.DrawLine(transform.position + Vector3.up * 1f, raySpreadEndPoint, Color.green);

        ray = new Ray(transform.position + Vector3.up * 1f, rayDirection);

        if (Physics.Raycast(ray, out RaycastHit rayHit, maxRay))
        {
            if (rayHit.transform.name == "Player_ch")
            {
                //Debug.Log(rayHit.transform.name);
                transform.LookAt(player.PlayerCurPosition());
                navAgent.SetDestination(player.PlayerCurPosition());
                isChasePlayerWithRay = true;

                if (Vector3.Distance(transform.position, player.PlayerCurPosition()) > eyeBeamRangeMin && eyeBeamCoolForCode == 0 && !isHitEyeBeam)
                {
                    isHitEyeBeam = true;
                    player.GetComponent<Player>().walkSpeed = player.GetComponent<Player>().walkSpeed * eyeBeamSpeedDown;
                    player.GetComponent<Player>().runSpeed = player.GetComponent<Player>().runSpeed * eyeBeamSpeedDown;
                }
            }
        }

        if (eyeBeamCoolForCode >= eyeBeamCoolForSetting)
        {
            eyeBeamCoolForCode = 0;
            isHitEyeBeam = false;
        }

        if (eyeBeamCoolForCode != 0)
        {
            ReturnNomalPlayerSpeed();
        }

        if (isHitEyeBeam)
        {
            eyeBeamCoolForCode += Time.deltaTime;
        }

        if (navAgent.remainingDistance < 0.2f)
        {
            isWaiting = true;
        }
    }


    //private void BetweenDistanceUnder500over300_1()
    //{
    //    navAgent.SetDestination(player.transform.position + new Vector3(Random.Range(-100, +100), player.transform.position.y, Random.Range(-100, +100)));
    //}
    //private void BetweenDistanceUnder500over300_2()
    //{
    //    return;
    //}
    //private void BetweenDistanceUnder500over300_3()
    //{
    //    transform.position = player.transform.position + new Vector3(Random.Range(-100, +100), player.transform.position.y, Random.Range(-100, +100));
    //}

    //private void BetweenDistanceUnder300over100_1()
    //{

    //}
    //private void BetweenDistanceUnder300over100_2()
    //{

    //}
    //private void BetweenDistanceUnder300over100_3()
    //{

    //}

    //private void BetweenDistanceUnder100()
    //{
    //    ChasePlayerWithRay();
    //}



}