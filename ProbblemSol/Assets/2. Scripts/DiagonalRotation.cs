using UnityEngine;

public class DiagonalRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // ȸ�� �ӵ�

    void Update()
    {
        // ȸ���� ���� ���
        float angle = rotationSpeed * Time.deltaTime;

        // �� �� �ֺ��� �밢������ ȸ���ϴ� ���� ����
        Vector3 rotationAxis = new Vector3(0.25f, 1f, 1f).normalized;

        // ���� ������Ʈ�� ȸ�� �������� ȸ��
        transform.Rotate(rotationAxis, angle);
    }
}
