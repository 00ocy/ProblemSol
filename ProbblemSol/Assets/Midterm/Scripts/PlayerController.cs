using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera Camera;
    public Transform target; // 바라볼 대상 오브젝트
    public float moveSpeed = 5f; // 이동 속도



    public Transform objectToRotate; // 회전시킬 오브젝트

    public float rotationSpeed = 90f;
    public float rotationDuration = 1f; // 변경에 걸리는 시간 (초)

    private bool isRotating = false;

    void Start()
    {

    }

    void Update()
    {
        // 이동 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 카메라의 전방 벡터를 기준으로 이동 방향 설정
        Vector3 cameraForward = Camera.transform.forward;
        cameraForward.y = 0f; // 상하 이동을 제거
        Vector3 moveDirection = cameraForward.normalized * moveZ + Camera.transform.right * moveX;

        // 이동
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

            // 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }


        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartCoroutine(RotateObject(1));
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(RotateObject(-1));
            }
        }
    }


    IEnumerator RotateObject(int direction)
    {
        isRotating = true;
        float elapsedTime = 0;
        Quaternion startRotation = objectToRotate.rotation;
        Quaternion targetRotation = Quaternion.Euler(objectToRotate.eulerAngles + new Vector3(0, direction * rotationSpeed, 0));

        while (elapsedTime < rotationDuration)
        {
            objectToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, (elapsedTime / rotationDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectToRotate.rotation = targetRotation;
        isRotating = false;
    }

}
