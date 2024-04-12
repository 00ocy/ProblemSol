using UnityEngine;

public class DiagonalRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도

    void Update()
    {
        // 회전할 각도 계산
        float angle = rotationSpeed * Time.deltaTime;

        // 각 축 주변의 대각선으로 회전하는 방향 설정
        Vector3 rotationAxis = new Vector3(0.25f, 1f, 1f).normalized;

        // 게임 오브젝트를 회전 방향으로 회전
        transform.Rotate(rotationAxis, angle);
    }
}
