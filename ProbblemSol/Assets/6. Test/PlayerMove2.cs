using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    private Vector3 moveDirection; // 이동 방향

    void Update()
    {
        // 이동 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // 이동 방향 설정
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // 이동 방향이 있을 때만 이동과 회전
        if (moveDirection != Vector3.zero)
        {
            // 이동
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
