using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private SettingManager settingManager;

    [SerializeField]
    private float turnSpeed = 4.0f;
    [SerializeField]
    private float moveSpeed = 2.0f;

    private Quaternion initialRotation;
    private float xRotate = 0.0f;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (SettingManager.isSettingMenuAct) return;
        
        MouseRotation();
    }

    private void MouseRotation()
    {
        float yRotate = transform.eulerAngles.y;
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -80, 60);

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
    }

    private void OnDisable() // 첫 시작에 마우스 위치 고정
    {
        transform.rotation = initialRotation;
    }
}