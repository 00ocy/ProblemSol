using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�
    private Vector3 moveDirection; // �̵� ����

    void Update()
    {
        // �̵� �Է� �ޱ�
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // �̵� ���� ����
        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // �̵� ������ ���� ���� �̵��� ȸ��
        if (moveDirection != Vector3.zero)
        {
            // �̵�
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // �̵� �������� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
