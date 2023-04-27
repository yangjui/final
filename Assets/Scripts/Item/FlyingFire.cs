using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingFire : MonoBehaviour
{
    //[SerializeField]
    //private SettingManager settingManager = null;
    //[SerializeField]
    //private OnHandManager onHandManager;

    [SerializeField]
    private float throwAngle = 30f;
    [SerializeField]
    private float throwForce = 8f;

    private GameObject fireExtinguisher;

    private void Start()
    {
        fireExtinguisher = OnHandManager.go;
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;

        if (OnHandManager.isFireOnHand && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire");
            fireExtinguisher.transform.parent = null;
            
            fireExtinguisher.GetComponent<Rigidbody>().isKinematic = false;
            fireExtinguisher.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 target = hit.point;
                ThrowBall(target);
                OnHandManager.isFireOnHand = false;
            }
            
            fireExtinguisher.GetComponent<Collider>().isTrigger = false;
        }
    }

    private void ThrowBall(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;
        float height = direction.y;

        float angle = throwAngle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(angle);

        distance += height / Mathf.Tan(angle);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angle));

        // 플레이어와 입력된 지점 사이의 방향벡터
        Vector3 launchDirection = direction.normalized;

        Vector3 launchVelocity = launchDirection * velocity;
        GetComponent<Rigidbody>().velocity = launchVelocity;
    }
}