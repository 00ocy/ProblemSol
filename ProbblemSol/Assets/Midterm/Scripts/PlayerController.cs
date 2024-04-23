using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera Camera;
    public Transform target; // 바라볼 대상 오브젝트
    public float moveSpeed = 5f; // 이동 속도
    public float rotationTime = 1f; // 회전 시간
    private Vector3[] cameraPositions; // 카메라 위치들
    private int currentCameraIndex = 0; // 현재 카메라 위치 인덱스
    private bool isCameraRotating = false; // 카메라 회전 중인지 여부

    void Start()
    {
        // 카메라 위치들 초기화
        cameraPositions = new Vector3[]
        {
            new Vector3(45f, 45f, 0f),
            new Vector3(45f, -45f, 0f),
            new Vector3(45f, -135f, 0f),
            new Vector3(45f, -225f, 0f)
        };

        // 초기 카메라 위치 설정
        SetCameraPosition(currentCameraIndex);
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

        // O 키 또는 P 키를 눌렀을 때 카메라 회전
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P))
        {
            if (!isCameraRotating) // 카메라가 회전 중이 아닌 경우에만 회전 수행
            {
                int newIndex = currentCameraIndex;
                if (Input.GetKeyDown(KeyCode.P))
                    newIndex = (currentCameraIndex + 1) % cameraPositions.Length; // 다음 위치로 이동
                else
                    newIndex = (currentCameraIndex - 1 + cameraPositions.Length) % cameraPositions.Length; // 이전 위치로 이동

                // 카메라 위치 변경
                SetCameraPosition(newIndex);
            }
        }
    }

    // 카메라의 위치를 변경하는 함수
    void SetCameraPosition(int index)
    {
        isCameraRotating = true; // 카메라 회전 중 상태로 변경

        // 이전 위치에서 현재 위치로 회전
        Quaternion startRotation = Camera.transform.rotation;
        // Vector3 startPosition = Camera.transform.position;
        Quaternion endRotation = Quaternion.Euler(cameraPositions[index]);
        // Vector3 endPosition = target.position - startRotation * Vector3.forward * 10f; // 대상 오브젝트를 바라보도록 설정

        // 회전과 이동 애니메이션 코루틴 시작
        StartCoroutine(RotateCamera(startRotation, endRotation));

        // 현재 카메라 위치 인덱스 갱신
        currentCameraIndex = index;
    }

    // 카메라의 회전 애니메이션을 처리하는 코루틴
    IEnumerator RotateCamera(Quaternion startRotation, Quaternion endRotation)
    {
        float elapsedTime = 0f;
        while (elapsedTime < rotationTime)
        {
            // 회전
            Camera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전 애니메이션 종료 후 회전 상태 해제
        isCameraRotating = false;
    }
}
